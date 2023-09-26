using Java.Net;
using Newtonsoft.Json;
using SmartHomeApp.Client;
using SmartHomeApp.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using Xamarin.Forms;
using static Android.Bluetooth.BluetoothClass;

namespace SmartHomeApp.Services
{
    class ConnectionDeviceService : IDisposable
    {

        private readonly RestClient _restClient = new RestClient();

        // Bei fehlerhaften Verbinden Verbindungsaufbau wiederholen und ggf. nach x-ten mal exception schmeißen.
        public async Task<bool?> IsShellyOnAsync(string ipAddress)
        {
            try
            {
                string url = $"http://{ipAddress}/status";

                var response = await _restClient.GetFromServerAsync(url);

                if (string.IsNullOrWhiteSpace(response))
                    return null;

                var statusResponse = JsonConvert.DeserializeObject<ShellyStatusResponse>(response);

                if (statusResponse == null)
                    return null;

                return statusResponse?.relays?.FirstOrDefault()?.isOn ?? null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<TogglingResponse> TurnToggleAsync(string ipAddress)
        {
            string url = $"http://{ipAddress}/relay/0?turn=toggle";
            string response;

            if (string.IsNullOrWhiteSpace(response = await _restClient.GetFromServerAsync(url)))
            {
                throw new Exception("Keine Verbindung zum Server 2");
            }

            TogglingResponse togglingResponse = JsonConvert.DeserializeObject<TogglingResponse>(response ?? string.Empty);
            return togglingResponse;
        }

        public async Task<bool> IsPingSuccessfull(string ipAddress, int port = 80, int timeoutMilliseconds = 2000)
        {
            using (var tcpClient = new TcpClient())
            {
                var taskConnect = tcpClient.ConnectAsync(ipAddress, port);

                if (await Task.WhenAny(taskConnect, Task.Delay(timeoutMilliseconds)) == taskConnect)
                {
                    await taskConnect;
                    return tcpClient.Connected;
                }
                else
                {
                    return false;  // Zeitüberschreitung
                }
            }
        }

        public async Task<double> GetPowerStatusAsync(string ipAddress)
        {
            string url = $"http://{ipAddress}/status";
            var jsonResponse = await _restClient.GetFromServerAsync(url);
            ShellyStatusResponse shellyPowerStatus = JsonConvert.DeserializeObject<ShellyStatusResponse>(jsonResponse);
            return shellyPowerStatus.meters.First().power;
        }

        public void Dispose()
        {
            //_restClient.Dispose();
        }
      
    }
}
