﻿using SmartHomeApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace SmartHomeApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
            BindingContext = new Services.Connection();
            
        }
    }
}