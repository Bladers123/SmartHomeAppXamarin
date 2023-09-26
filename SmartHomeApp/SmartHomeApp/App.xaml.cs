using SmartHomeApp.Client;
using SmartHomeApp.Services;
using SmartHomeApp.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartHomeApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.RegisterSingleton<ConnectionDeviceService>(new ConnectionDeviceService());
            DependencyService.Register<IConnectionService, ConnectionService>();
            
            MainPage = new AppShell();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;    
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;    // Für asynchrone Tasks

        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Loggen oder behandeln Sie die Exception hier
            Debug.WriteLine(e.ExceptionObject.ToString());
        }


        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            // Loggen oder behandeln Sie die Exception hier
            Debug.WriteLine(e.Exception.ToString());
        }


        protected override void OnStart()
        {
           
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
        }
    }
}
