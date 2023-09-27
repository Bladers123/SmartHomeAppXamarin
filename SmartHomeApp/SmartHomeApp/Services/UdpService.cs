using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SmartHomeApp.Services
{
    public class UdpService
    {
        public async Task<string> SendUdpBroadcastAsync(string message, int port)
        {
            using (var client = new UdpClient())
            {
                try
                {
                    client.EnableBroadcast = true;
                    var requestData = Encoding.UTF8.GetBytes(message);
                    var serverEndpoint = new IPEndPoint(IPAddress.Broadcast, port);

                    await client.SendAsync(requestData, requestData.Length, serverEndpoint);

                    var task = client.ReceiveAsync();

                    if (await Task.WhenAny(task, Task.Delay(5000)) == task) // 5 Sekunden Timeout
                    {
                        var result = task.Result;
                        return Encoding.UTF8.GetString(result.Buffer);
                    }
                    else
                    {
                        throw new TimeoutException("UDP receive timed out.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred: {ex}");
                    return null;
                }
            }
        }
    }
}
