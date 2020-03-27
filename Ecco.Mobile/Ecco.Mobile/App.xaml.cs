using Ecco.Api;
using Ecco.Mobile.Views;
using Nancy.TinyIoc;
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
            MainPage = new Login();
        }

        private void InitDatabase()
        {
            string ip = "172.26.164.74";
            string baseUrl = "https://" + ip + ":44355";
            var container = TinyIoCContainer.Current;
            container.Register<IDatabaseManager, DatabaseManager>();
            container.Resolve<IDatabaseManager>().SetUrl(baseUrl);
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
