using Ecco.Api;
using Ecco.Mobile.Views;
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
            InitDatabase();
            MainPage = new LoadingPage();
            if (!CrossSettings.Current.GetValueOrDefault("Username", "_").Equals("_"))
            {
                AutoLogin();
            }
            else
            {
                MainPage = new Login();
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
                MainPage = new Login();
                await MainPage.DisplayAlert("Authentication Error", "You have been logged out", "Ok");
            }
        }

        private void InitDatabase()
        {
            var container = TinyIoCContainer.Current;
            container.Register<IDatabaseManager, DatabaseManager>();
            container.Resolve<IDatabaseManager>().SetUrl("https://ecco-space.azurewebsites.net/");
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
