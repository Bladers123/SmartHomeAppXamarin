using System.Windows.Input;
using Xamarin.Forms;
using SmartHomeApp.Views;
using System.Threading.Tasks;

namespace SmartHomeApp.ViewModels
{
    internal class StartSideViewModel
    {
        public ICommand StartCommand { get; private set; }
        public StartSideViewModel() 
        {
            StartCommand = new Command(async () => await ExecuteStartCommand());
        }

        private async Task ExecuteStartCommand()
        {
           await Application.Current.MainPage.Navigation.PushAsync(new ItemsPage());
        }
    }
}