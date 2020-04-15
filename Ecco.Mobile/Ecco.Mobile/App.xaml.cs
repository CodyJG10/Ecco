using Ecco.Api;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Views;
using Ecco.Mobile.Views.Authentication;
using Nancy.TinyIoc;
using Plugin.Settings;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM1NDA2QDMxMzgyZTMxMmUzMGZGWjB2Z0prb1hpcCtLTDZPVUo3bXhnUGFnV0VRcTZCUEk3UXZZdFB5M1U9");

            InitDatabase();
            

            MainPage = new LoadingPage();
            DependencyService.Get<INFCReader>().ReadTag();
            //if (!CrossSettings.Current.GetValueOrDefault("Username", "_").Equals("_"))
            //{
            //    AutoLogin();
            //}
            //else
            //{
            //    MainPage = new LoginPage();
            //}
        }

        private async void AutoLogin()
        {
            string username = CrossSettings.Current.GetValueOrDefault("Username", "");
            string password = CrossSettings.Current.GetValueOrDefault("Password", "");
            var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            var loginSuccesful = await db.Login(username, password);
            if (loginSuccesful)
            {
                MainPage = new NavigationPage(new Home());
            }
            else
            {
                MainPage = new LoginPage();
                await MainPage.DisplayAlert("Authentication Error", "You have been logged out", "Ok");
            }
        }

        private void InitDatabase()
        {
            var container = TinyIoCContainer.Current;
            container.Register<IDatabaseManager, DatabaseManager>();
            container.Resolve<IDatabaseManager>().SetUrl("https://ecco-space.azurewebsites.net/");

            container.Register<IStorageManager>(new StorageManager("DefaultEndpointsProtocol=https;AccountName=eccospacestorage;AccountKey=Nr6eERil/QqRitQ/XThQ9yElPlH844fwAqE0LDOX6ktyYae0S5xtvv5W/d0lrM3Y7uI8KP7qRgoQ/unHCmYnIw==;EndpointSuffix=core.windows.net"));
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
