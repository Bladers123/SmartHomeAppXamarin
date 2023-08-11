using SmartHomeApp.ViewModels;
using SmartHomeApp.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace SmartHomeApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }
    }
}
