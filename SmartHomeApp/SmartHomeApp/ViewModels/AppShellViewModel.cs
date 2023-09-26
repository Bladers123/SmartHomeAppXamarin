using SmartHomeApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using SmartHomeApp.Services;

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
