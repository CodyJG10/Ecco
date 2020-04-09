using Ecco.Api;
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
            if (!CrossSettings.Current.GetValueOrDefault("Username", "_").Equals("_"))
            {
                AutoLogin();
            }
            else
            {
                MainPage = new LoginPage();
            }
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

            container.Register<IStorageManager>(new StorageManager("DefaultEndpointsProtocol=https;AccountName=eccodevstorage;AccountKey=8N0q/fS/d3cuNl48eleenu2EdU0VN3crTFUIegdVkBCP5wY1URNmC+XPBon8bOCShk8Ku8HV7CsmXm2cS7BUOg==;EndpointSuffix=core.windows.net"));
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
