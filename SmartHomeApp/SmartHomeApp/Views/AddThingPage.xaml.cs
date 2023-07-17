using SmartHomeApp.Models;
using SmartHomeApp.ViewModels;
using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartHomeApp.Views
{
    public partial class AddThingPage : ContentPage
    {
        public AddThingPage()
        { 
            InitializeComponent();
            BindingContext = new ViewModels.AddThingPageViewModel();
        }
    }
}