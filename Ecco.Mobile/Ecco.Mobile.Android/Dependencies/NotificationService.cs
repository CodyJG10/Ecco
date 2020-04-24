using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ecco.Mobile.Droid.Dependencies.NotificationService))]
namespace Ecco.Mobile.Droid.Dependencies
{
    public class NotificationService : INotificationRegister
    {
        public async void RegisterForRemoteNotifications(string username, IDatabaseManager db)
        {
            var context = Android.App.Application.Context;
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            if (prefs.Contains("DeviceToken"))
            {
                var token = prefs.GetString("DeviceToken", "");
                var registrationId = await db.RegisterDevice();
                DeviceRegistration deviceUpdate = new DeviceRegistration()
                {
                    Handle = token,
                    Platform = "fcm",
                    Tags = new string[] { "username:" + username }
                };
                await db.EnablePushNotifications(registrationId, deviceUpdate);
            }
        }
    }
}