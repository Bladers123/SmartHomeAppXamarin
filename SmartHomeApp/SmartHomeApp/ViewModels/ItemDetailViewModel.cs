using SmartHomeApp.Client;
using SmartHomeApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {

        ConnectionThingService connectionThingService;
        private bool updatePowerStatusIsRunning;
        public ItemDetailViewModel()
        {
            Title = "Gerät";
            IsButtonEnabled = true;
            this.updatePowerStatusIsRunning = false;
            AddDeviceCommand = new Command(async () => await OnDevice());
            connectionThingService = DependencyService.Get<ConnectionThingService>();
            // this.shellyIpAddress = "192.168.188.45"; // Schule 192.168.216.116 -- Zuhause 192.168.188.45
        }

        private void AfterLoadId()
        {
            CanConnectToService();
            StartUpdatingPowerStatus();
        }

        public ICommand AddDeviceCommand { get; }

        public async Task OnDevice()
        {
            try
            {
                IsButtonEnabled = false;

                // prüft ob die validierung stimmt
                if (!IPAddress.TryParse(Ip, out IPAddress ipAddress))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Ip ungültig.", "OK");
                    await setButtonEnablePropertyOnTrue();
                    return;
                }

                // prüft ob die Adresse erreichbar ist
                else if (!await connectionThingService.IsPingSuccessfull(Ip))
                {
                    await Shell.Current.DisplayAlert("Fehler", "Nicht im lokalen Netzwerk erreichbar!", "OK");
                    await setButtonEnablePropertyOnTrue();
                    return;
                }

                // prüft ob ein und ausgeschaltet werden kann
                if (await connectionThingService.TurnToggleAsync(Ip) == null)
                {
                    await Shell.Current.DisplayAlert("Fehler", "Die Shelly-Steckdose konnte nicht geschaltet werden.", "OK");
                    await setButtonEnablePropertyOnTrue();
                    return;
                }
            }
            // j
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
                await setButtonEnablePropertyOnTrue();
            }

            await setButtonEnablePropertyOnTrue();
        }

        private async Task setButtonEnablePropertyOnTrue()
        {
            await Task.Delay(1000);
            IsButtonEnabled = true;
        }

        private async Task UpdatePowerStatus()
        {
            if (updatePowerStatusIsRunning)
                return;

            updatePowerStatusIsRunning = true;

            try
            {
                double powerStatus = await connectionThingService.GetPowerStatusAsync(Ip);
                WattLabel = powerStatus.ToString() + " W";
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
            finally
            {
                updatePowerStatusIsRunning = false;
            }
        }

        private void StartUpdatingPowerStatus()
        {
            Device.StartTimer(TimeSpan.FromSeconds(4), () =>
            {
                if (!updatePowerStatusIsRunning)
                    UpdatePowerStatus();

                return true; // Hiermit stellen Sie sicher, dass der Timer weiterläuft
            });
        }

        private void CanConnectToService()
        {
            var connectionService = DependencyService.Get<IConnectionService>();

            connectionService.StartCheckingConnection(Ip, (canConnect) =>
            {
                if (canConnect)
                {
                    // Verbindung erfolgreich
                }
                else
                {
                    // Verbindung fehlgeschlagen
                }
            });

            // Um die Überprüfung zu stoppen:
            // connectionService.StopCheckingConnection();
        }





        private string itemId;
        private string name;
        private string description;
        private string ip;

        public string Id { get; set; }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

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
                AfterLoadId();
            }
        }
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
