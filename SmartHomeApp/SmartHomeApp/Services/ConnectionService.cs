using SmartHomeApp.Client;
using System;
using System.Threading;
using System.Threading.Tasks;



public interface IConnectionService
{
    Task<bool> CanConnectToUrlAsync(string url);
    void StartCheckingConnection(string url, Action<bool> onConnectionCheck);
    void StopCheckingConnection();
}

public class ConnectionService : IConnectionService, IDisposable
{
    private readonly RestClient restClient;
    private Timer _connectionCheckTimer;

    public ConnectionService()
    {
        restClient = new RestClient();
    }

    public async Task<bool> CanConnectToUrlAsync(string url)
    {
        try
        {
            await restClient.GetFromServerAsync($"http://{url}/status");
            return true;
        }
        catch (Exception )
        {
            return false;
        }
    }

    public void StartCheckingConnection(string url, Action<bool> onConnectionCheck)
    {
        _connectionCheckTimer = new Timer(async _ =>
        {
            var canConnect = await CanConnectToUrlAsync(url);
            onConnectionCheck(canConnect);
        },
        null,
        TimeSpan.Zero, // Start sofort
        TimeSpan.FromSeconds(10)); // Alle 2 Sekunden wiederholen
    }

    public void StopCheckingConnection()
    {
        _connectionCheckTimer?.Dispose();
    }

    public void Dispose()
    {
       // restClient.Dispose();
    }
}
