using System.Windows.Input;
using Xamarin.Forms;


namespace SmartHomeApp.ViewModels
{
    class AppShellViewModel
    {
        public AppShellViewModel()
        {
            
        }

        public ICommand NavigateToNewItemPageCommand => new Command(async () =>
        await Shell.Current.GoToAsync($"//ItemsPage/NewItemPage"));     
    }
}
