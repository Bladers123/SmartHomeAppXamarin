using Newtonsoft.Json;
using SmartHomeApp.Client;
using SmartHomeApp.Models;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;




namespace SmartHomeApp.Services
{
    class ConnectionThingService
    {

        private readonly RestClient _restClient = new RestClient();


        public async Task<TogglingResponse> TurnToggleAsync(string ipAddress)
        {
            string url = $"http://{ipAddress}/relay/0?turn=toggle";
            string response = await _restClient.GetFromServerAsync(url);
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
                    await taskConnect;  // Stellen Sie sicher, dass etwaige Ausnahmen ausgelöst werden.
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
            ShellyPowerStatus shellyPowerStatus = JsonConvert.DeserializeObject<ShellyPowerStatus>(jsonResponse);
            return shellyPowerStatus.meters.First().power;
        }

    }
}
