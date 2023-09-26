using SmartHomeApp.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartHomeApp.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            var vm = BindingContext as ItemDetailViewModel;

            var newValue = e.Value;
            var oldValue = !newValue;     

            vm.ToggledHasChanged(oldValue,newValue);
        }
    }
}