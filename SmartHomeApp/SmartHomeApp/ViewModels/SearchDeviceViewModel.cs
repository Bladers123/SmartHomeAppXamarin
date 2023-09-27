using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Zeroconf;
using SmartHomeApp.Models;
using System;
using System.Linq;
using Rssdp;
using static Android.Bluetooth.BluetoothClass;
using System.Net.Http;
using SmartHomeApp.Client;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using SmartHomeApp.Services;

//namespace SmartHomeApp.ViewModels
//{
//    public class SearchDeviceViewModel : INotifyPropertyChanged
//    {
//        public ICommand SearchDevice { get; }
//        public ObservableCollection<ShellyDevice> Devices { get; set; }

//        public event PropertyChangedEventHandler PropertyChanged;


//        public SearchDeviceViewModel()
//        {
//            SearchDevice = new Command(async () => await OnSearchButton());
//            Devices = new ObservableCollection<ShellyDevice>();
//            Devices.Add(new ShellyDevice { Name = "Test Device 1", IPAddress = "192.168.1.1" });
//            Devices.Add(new ShellyDevice { Name = "Test Device 2", IPAddress = "192.168.1.2" });
//        }


//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }

//        public async Task OnSearchButton()
//        {
//            try
//            {
//                var domains = await ZeroconfResolver.BrowseDomainsAsync();
//                Console.WriteLine($"Found {domains.Count()} domains"); // Debug-Ausgabe

//                var resolvedDevices = await ZeroconfResolver.ResolveAsync("http://localhost/");


//                foreach (var domainGroup in domains)
//                {
//                    var domain = domainGroup.Key; // Hier erhalten wir den Domain-String
//                    Console.WriteLine($"Domain: {domain}"); // Debug-Ausgabe für jede Domain

//                    foreach (var ip in domainGroup) // Iterieren über die Elemente der Gruppe
//                    {
//                        var shellyDevice = new ShellyDevice { Name = domain, IPAddress = ip }; // Beispiel
//                        Devices.Add(shellyDevice);
//                        Console.WriteLine($"Added device {shellyDevice.Name} with IP {shellyDevice.IPAddress}"); // Debug-Ausgabe
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error occurred: {ex}");
//            }
//        }


//        //public async Task OnSearchButton()
//        //{
//        //    try
//        //    {
//        //        //Erstellen Sie einen Device Locator
//        //        var deviceLocator = new SsdpDeviceLocator();

//        //        //Suchen Sie nach allen SSDP - Geräten
//        //        var foundDevices = await deviceLocator.SearchAsync();

//        //        //Fügen Sie die gefundenen Geräte zur Devices - Liste hinzu
//        //        foreach (var foundDevice in foundDevices)
//        //        {
//        //            var fullDevice = await foundDevice.GetDeviceInfo();

//        //            var shellyDevice = new ShellyDevice
//        //            {
//        //                Name = fullDevice.FriendlyName,
//        //                IPAddress = foundDevice.DescriptionLocation.Host
//        //            };

//        //            Devices.Add(shellyDevice);

//        //            Console.WriteLine($"Added device {shellyDevice.Name} with IP {shellyDevice.IPAddress}");
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine($"Error occurred: {ex}");
//        //    }
//        //}
//    }
//}

//    public class SearchDeviceViewModel : INotifyPropertyChanged
//    {
//        public ICommand SearchDevice { get; }
//        public ObservableCollection<ShellyDevice> Devices { get; set; }

//        private readonly MqttService _mqttService;

//        public event PropertyChangedEventHandler PropertyChanged;

//        public SearchDeviceViewModel()
//        {
//            SearchDevice = new Command(async () => await OnSearchButton());
//            Devices = new ObservableCollection<ShellyDevice>();
//            _mqttService = new MqttService();
//            _mqttService.MessageReceived += OnMessageReceived;
//        }

//        public async Task OnSearchButton()
//        {
//            try
//            {
//                var topic = "shellies/shellyplugs-<deviceid>/relay/<i>"; // Du musst das richtige Topic hier setzen.
//                await _mqttService.Subscribe(topic);
//            }
//            catch (Exception ex)
//            {
//                Debug.WriteLine($"Error occurred: {ex}");
//            }
//        }

//        private void OnMessageReceived(string message)
//        {
//            // Hier kannst du die Nachricht verarbeiten und deine Geräteliste aktualisieren
//            Debug.WriteLine($"Received message: {message}");

//            // Zum Beispiel, wenn deine Nachricht Informationen über das Gerät enthält:
//            var shellyDevice = new ShellyDevice
//            {
//                // Hier musst du die richtigen Werte aus der Nachricht extrahieren.
//                Name = "Shelly Device",
//                IPAddress = "192.168.1.100"
//            };
//            Devices.Add(shellyDevice);
//        }

//        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//        {
//            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }

//public class SearchDeviceViewModel : INotifyPropertyChanged
//{
//    public ICommand SearchDevice { get; }
//    public ObservableCollection<ShellyDevice> Devices { get; set; }

//    public event PropertyChangedEventHandler PropertyChanged;


//    public SearchDeviceViewModel()
//    {
//        SearchDevice = new Command(async () => await OnSearchButton());
//        Devices = new ObservableCollection<ShellyDevice>();
//        Devices.Add(new ShellyDevice { Name = "Test Device 1", IPAddress = "192.168.1.1" });
//        Devices.Add(new ShellyDevice { Name = "Test Device 2", IPAddress = "192.168.1.2" });
//    }


//    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
//    {
//        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//    }

//    //public async Task OnSearchButton()
//    //{
//    //    try
//    //    {
//    //        var domains = await ZeroconfResolver.BrowseDomainsAsync();
//    //        Console.WriteLine($"Found {domains.Count()} domains"); // Debug-Ausgabe

//    //        var resolvedDevices = await ZeroconfResolver.ResolveAsync("http://localhost/");


//    //        foreach (var domainGroup in domains)
//    //        {
//    //            var domain = domainGroup.Key; // Hier erhalten wir den Domain-String
//    //            Console.WriteLine($"Domain: {domain}"); // Debug-Ausgabe für jede Domain

//    //            foreach (var ip in domainGroup) // Iterieren über die Elemente der Gruppe
//    //            {
//    //                var shellyDevice = new ShellyDevice { Name = domain, IPAddress = ip }; // Beispiel
//    //                Devices.Add(shellyDevice);
//    //                Console.WriteLine($"Added device {shellyDevice.Name} with IP {shellyDevice.IPAddress}"); // Debug-Ausgabe
//    //            }
//    //        }
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        Console.WriteLine($"Error occurred: {ex}");
//    //    }
//    //}


//    //public async Task OnSearchButton()
//    //{
//    //    try
//    //    {
//    //        // Erstellen Sie einen Device Locator
//    //        var deviceLocator = new SsdpDeviceLocator();

//    //        // Suchen Sie nach allen SSDP-Geräten
//    //        var foundDevices = await deviceLocator.SearchAsync();

//    //        // Fügen Sie die gefundenen Geräte zur Devices-Liste hinzu
//    //        foreach (var foundDevice in foundDevices)
//    //        {
//    //            var fullDevice = await foundDevice.GetDeviceInfo();

//    //            var shellyDevice = new ShellyDevice
//    //            {
//    //                Name = fullDevice.FriendlyName,
//    //                IPAddress = foundDevice.DescriptionLocation.Host
//    //            };

//    //            Devices.Add(shellyDevice);

//    //            Console.WriteLine($"Added device {shellyDevice.Name} with IP {shellyDevice.IPAddress}");
//    //        }
//    //    }
//    //    catch (Exception ex)
//    //    {
//    //        Console.WriteLine($"Error occurred: {ex}");
//    //    }
//    //}



namespace SmartHomeApp.ViewModels
{
    public class SearchDeviceViewModel : INotifyPropertyChanged
    {
        private readonly UdpService _udpService;

        public ICommand SearchDevice { get; }
        public ObservableCollection<ShellyDevice> Devices { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public SearchDeviceViewModel()
        {
            _udpService = new UdpService();
            SearchDevice = new Command(async () => await OnSearchButton());
            Devices = new ObservableCollection<ShellyDevice>();
        }

        public async Task OnSearchButton()
        {
            try
            {
                string message = ""; // Ihr UDP-Nachricht hier
                int port = 0; // Ihr UDP-Port hier

                var response = await _udpService.SendUdpBroadcastAsync(message, port);

                if (!string.IsNullOrEmpty(response))
                {                   
                     Devices.Add(new ShellyDevice { Name = "Name aus Antwort", IPAddress = "IP aus Antwort" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex}");
            }
        }

        // ... restlichen Methoden und Eigenschaften des ViewModels ...
    }
}
