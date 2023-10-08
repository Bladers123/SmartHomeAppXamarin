using SmartHomeApp.Models;
using System;
using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string name;
        private string description;
        private string ip;

    

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        // Abfrage ob die Felder leer sind
        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name)
                && !String.IsNullOrWhiteSpace(ip);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string Ip
        {
            get => ip;
            set => SetProperty(ref ip, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }


        // Schließt das Fenster
        private async void OnCancel()
        {
            await Shell.Current.GoToAsync($"..");
        }

        private async void OnSave()
        {
            Item newItem = new Item()
            {
                Id = Guid.NewGuid().ToString(),
                Name = Name,
                Description = Description,
                Ip = Ip
            };

            await DataStore.AddItemAsync(newItem);

            await Shell.Current.GoToAsync($"..");
        }
    }
}
