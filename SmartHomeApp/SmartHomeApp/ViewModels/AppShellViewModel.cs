using SmartHomeApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    class AppShellViewModel
    {
           public ICommand NavigateToNewItemPageCommand => new Command(async () =>
           await Shell.Current.GoToAsync($"//ItemsPage/NewItemPage"));


        // Andere Schreibweise 
       /* public Command NavigateToNewItemPageCommand { get; }

        public AppShellViewModel()
        {
            NavigateToNewItemPageCommand = new Command(hi);
        }     
        private async void hi(object obj)
        {
            await Shell.Current.GoToAsync($"//ItemsPage/NewItemPage");
        }*/
    }
}
