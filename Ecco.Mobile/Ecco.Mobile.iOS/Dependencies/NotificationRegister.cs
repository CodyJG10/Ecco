using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Dependencies;
using Foundation;
using UIKit;
using WindowsAzure.Messaging;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ecco.Mobile.iOS.Dependencies.NotificationRegister))]
namespace Ecco.Mobile.iOS.Dependencies
{
    public class NotificationRegister : INotificationRegister
    {
        //        public async void RegisterForRemoteNotifications(string username, IDatabaseManager db, string installationId = null)
        //        {
        //            //var app = (AppDelegate)UIApplication.SharedApplication.Delegate;
        //            //var deviceToken = app.DeviceToken;
        //            //if (deviceToken != null)
        //            //{
        //            //    DeviceRegistration registration = new DeviceRegistration()
        //            //    {
        //            //        InstallationId = deviceToken.
        //            //    };

        //            //    db.RegisterDevice()
        //            //}



        //            if (installationId == null)
        //            {
        //                installationId = Guid.NewGuid().ToString();
        //            }

        //            var pushNotificationChannel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

        //        }
        public void RegisterForRemoteNotifications(string username)
        {
            var app = (AppDelegate)UIApplication.SharedApplication.Delegate;
            app.RegisterForRemoteNotifications(username);
        }
    }
}