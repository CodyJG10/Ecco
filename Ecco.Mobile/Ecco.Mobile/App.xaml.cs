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
