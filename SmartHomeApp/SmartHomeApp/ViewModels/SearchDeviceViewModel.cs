using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHomeApp.Models;


namespace SmartHomeApp.ViewModels
{
    public class SearchDeviceViewModel : INotifyPropertyChanged
    {
        public ICommand SearchDevice { get; }
        public ObservableCollection<ShellyDevice> Devices { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public SearchDeviceViewModel()
        {
            SearchDevice = new Command(OnSearchButton);
            Devices = new ObservableCollection<ShellyDevice>();          
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnSearchButton()
        {
            Devices.Add(new ShellyDevice { Name = "Test-Gerät 1", IPAddress = "192.168.1.1" });
            Devices.Add(new ShellyDevice { Name = "Test-Gerät 2", IPAddress = "192.168.1.2" });
        }
    }
}