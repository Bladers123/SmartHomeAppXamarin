using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    public class ThingsViewModel : BaseViewModel
    {

        //Stellt eine Klasse zum Senden von HTTP-Anforderungen und Empfangen von HTTP-Antworten
        private readonly HttpClient httpClient;
        private readonly string shellyIpAddress;

        public ThingsViewModel()
        {
            Title = "Geräte";
            AddDeviceCommand = new Command(OnAddDevice);

            this.httpClient = new HttpClient();
            this.shellyIpAddress = "192.168.216.116";
        }


        public ICommand AddDeviceCommand { get; }

        public async void OnAddDevice()
        {
            try
            {
                var url = $"http://{this.shellyIpAddress}/relay/0?turn=toggle"; // Falls Authentifizierung erforderlich ist, fügen Sie "&auth_key=YOUR_AUTH_KEY" am Ende der URL hinzu.
                var response = await this.httpClient.GetAsync(url);     // Fragt das Gerät an

                // war die response (Antwort) vom request (Anfrage) nicht erfolgreich?
                if (!response.IsSuccessStatusCode)
                {
                    await Shell.Current.DisplayAlert("Fehler", "Die Shelly-Steckdose konnte nicht geschaltet werden.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Ein Fehler ist aufgetreten: {ex.Message}", "OK");
            }
        }
    }
}