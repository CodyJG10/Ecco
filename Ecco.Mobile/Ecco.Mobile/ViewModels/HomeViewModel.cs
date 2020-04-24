using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Dependencies;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
            RegisterForPushNotifications();
            AutoUpdater autoUpdater = new AutoUpdater();
            TinyIoCContainer.Current.Register(autoUpdater);
            autoUpdater.Start();
        }

        void RegisterForPushNotifications()
        {
            var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            IDatabaseManager db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            DependencyService.Get<INotificationRegister>().RegisterForRemoteNotifications(userData.UserName, db);
        }
    }
}