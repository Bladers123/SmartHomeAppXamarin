using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            TestCommand = new Command(OnTest);
        }
  

        public ICommand TestCommand { get; }

        public void OnTest()
        {
            Shell.Current.DisplayAlert("Hey", "Meine Nachricht", "Ok");
        }
    }
}