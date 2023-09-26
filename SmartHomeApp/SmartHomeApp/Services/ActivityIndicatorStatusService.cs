using System;
using System.ComponentModel;

namespace SmartHomeApp.Services
{
    public class ActivityIndicatorStatusService : INotifyPropertyChanged
    {

        private static readonly Lazy<ActivityIndicatorStatusService> lazyInstance =
       new Lazy<ActivityIndicatorStatusService>(() => new ActivityIndicatorStatusService());

        public static ActivityIndicatorStatusService Instance => lazyInstance.Value;


        private bool isRunning;
        public bool IsRunning
        {
            get => isRunning;
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged(nameof(IsRunning));
                }
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                if (isVisible != value)
                {
                    isVisible = value;
                    OnPropertyChanged(nameof(IsVisible));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static void IsActivityIndicatorOn()
        {
            Instance.IsRunning = true;
            Instance.IsVisible = true;
        }

        public static void IsActivityIndicatorOff()
        {
            Instance.IsRunning = false;
            Instance.IsVisible = false;
        }
    }
}
