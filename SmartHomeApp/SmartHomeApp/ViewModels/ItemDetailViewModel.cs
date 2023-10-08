using SmartHomeApp.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace SmartHomeApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        ConnectionDeviceService connectionThingService;
        private bool updatePowerStatusIsRunning;
        private bool initDone = false;

        public ItemDetailViewModel()
        {
            connectionThingService = DependencyService.Get<ConnectionDeviceService>();
        }

        public async Task SetDeviceStateToSwitch()
        {
            bool? result = null;
            int maxIterations = 3;
            int iterator = 0;

            while (result == null && iterator < maxIterations)
            {
                result = await connectionThingService.IsShellyOnAsync(Ip);
                iterator++;
            }

            if (result == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Verbindung nicht möglich", "OK");
                return;
            }

            else
                IsSwitchedOn = (bool)result;

            if (IsSwitchedOn == false)
                initDone = true;
        }

        private bool _isSwitchedOn;
        public bool IsSwitchedOn
        {
            get => _isSwitchedOn;
            set
            {
                _isSwitchedOn = value;
                OnPropertyChanged(nameof(IsSwitchedOn));
            }
        }

        private string _onOffLabel = "Aus";
        public string OnOffLabel
        {
            get => _onOffLabel;
            set
            {
                _onOffLabel = value;
                OnPropertyChanged(nameof(OnOffLabel));
            }
        }

        public void ToggledHasChanged(bool oldValue, bool newValue)
        {
            // Hier gehe ich rein, wenn Steckdose an ist und ich gerade das erste mal aufgerufen werde
            if (!initDone && newValue) { }
            else
            {
                Task.Run(async () => await TurnToggleAsync());
            }
            OnOffLabel = newValue ? "Ein" : "Aus";
            initDone = true;
        }

        private async Task AfterLoadId()
        {
            CanConnectToService();  
            StartUpdatingPowerStatus();

            var task = Task.Run(async () =>
            {
                try
                {
                    await SetDeviceStateToSwitch();
                }
                catch
                {
                    return false;
                }

                return true;
            });
            // Verwende 'await', um auf das Ergebnis zu warten, ohne den Thread zu blockieren.
            bool result = await task;

            if (result == false)
            {
                await Shell.Current.DisplayAlert("Fehler", "Verbindung zum Shelly-Gerät nicht möglich.", "OK");
                await Shell.Current.Navigation.PopAsync();
            }
        }

        public async Task TurnToggleAsync()
        {
            if (await connectionThingService.TurnToggleAsync(Ip) == null)
            {
                await Shell.Current.DisplayAlert("Fehler", "Die Shelly-Steckdose konnte nicht geschaltet werden.", "OK");
                return;
            }
        }

        // TODO könnte einen gründlicheren Check haben. 
        public async Task<bool> DeviceCheckAsync(string Ip)
        {
            try
            {
                // prüft ob die validierung stimmt
                if (!IPAddress.TryParse(Ip, out IPAddress ipAddress))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Ip ungültig.", "OK");
                    return false;
                }

                // prüft ob die Adresse erreichbar ist
                if (!await connectionThingService.IsPingSuccessfull(Ip))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Nicht im lokalen Netzwerk erreichbar!", "OK");
                    return false;
                }

                return true;
            }
            catch (HttpRequestException ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Verbindung zum Server konnte nicht hergestellt werden. " + ex.Message, "OK");
                return false;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", "Etwas ist schief gelaufen: " + ex.Message, "OK");
                return false;
            }
        }

        string wattLabel = string.Empty;
        public string WattLabel
        {
            get { return wattLabel; }
            set
            {
                SetProperty(ref wattLabel, value);
            }
        }

        private async Task UpdatePowerStatus()
        {
            if (updatePowerStatusIsRunning)
                return;

            updatePowerStatusIsRunning = true;

            try
            {
                double powerStatus = await connectionThingService.GetPowerStatusAsync(Ip);
                WattLabel = powerStatus.ToString() + " Watt";
            }
            catch (TargetInvocationException tie)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist in der Methode 'UpdatePowerStatus' aufgetreten: {tie.InnerException.Message}", "OK");

            }
            finally
            {
                updatePowerStatusIsRunning = false;
            }
        }

        private void StartUpdatingPowerStatus()
        {
            Xamarin.Forms.Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                if (!updatePowerStatusIsRunning)
                {
                    var task = Task.Run(async () => { await UpdatePowerStatus(); });

                    if (task.Exception != null)
                    {
                        try
                        {
                            task.Wait();
                        }
                        catch (Exception)
                        {
                        }
                    }
                }

                return true; // Hiermit stell ich sicher, dass der Timer weiterläuft
            });
        }

        // TODO falls innerException Fehler
        private void CanConnectToService()
        {
            var connectionService = DependencyService.Get<IConnectionService>();

            connectionService.StartCheckingConnection(Ip, async (canConnect) =>
            {
                if (canConnect)
                {
                    return;// Verbindung erfolgreich
                }
                else
                {
                    await Shell.Current.DisplayAlert("Fehler", "Fehler beim Verbinden des Services.", "OK"); // Verbindung fehlgeschlagen
                    return;
                }
            });

            // Um die Überprüfung zu stoppen:
            connectionService.StopCheckingConnection();
        }



        public string Id { get; set; }


        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }


        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }


        private string itemId;
        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
                Task.Run(async () => await AfterLoadId()); 
            }
        }

        private string ip;
        public string Ip
        {
            get => ip;
            set => SetProperty(ref ip, value);
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Name = item.Name;
                Description = item.Description;
                Ip = item.Ip;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Fehler beim Laden der Items: {ex.Message}", "OK");
            }
        }
    }
}
