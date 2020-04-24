using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public async void RegisterForRemoteNotifications(string username, IDatabaseManager db)
        {
            var app = (AppDelegate)UIApplication.SharedApplication.Delegate;
            if (app.PushNotificationsHandle != null)
            {

                var id = await db.RegisterDevice();
                var handle = app.PushNotificationsHandle;
                DeviceRegistration deviceUpdate = new DeviceRegistration()
                {
                    Handle = handle,
                    Platform = "apns",
                    Tags = new string[] { "username:" + username }
                };

                var result = await db.EnablePushNotifications(id, deviceUpdate);
                Console.WriteLine(result);
            }
        }
    }
}