using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Ecco.Web.Areas.Identity;
using Ecco.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Azure.NotificationHubs;

namespace Ecco.Web.Services
{
    public class NotificationService
    {
        private NotificationHubClient _hub;

        public NotificationService(string hubName, string connectionString)
        {
            _hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, hubName);
        }

        public async Task<bool> RegisterForPushNotifications(string id, DeviceRegistration deviceUpdate, UserManager<EccoUser> userManager)
        {
            RegistrationDescription registrationDescription = null;
            int deviceType = 0;

            switch (deviceUpdate.Platform)
            {
                case "apns":
                    registrationDescription = new AppleRegistrationDescription(deviceUpdate.Handle, deviceUpdate.Tags);
                    deviceType = DeviceTypeConstants.IOS;
                    break;
                case "fcm":
                    registrationDescription = new FcmRegistrationDescription(deviceUpdate.Handle, deviceUpdate.Tags);
                    deviceType = DeviceTypeConstants.ANDROID;
                    break;
            }

            registrationDescription.RegistrationId = id;
            if (deviceUpdate.Tags != null)
                registrationDescription.Tags = new HashSet<string>(deviceUpdate.Tags);

            try
            {
                var user = await userManager.FindByNameAsync(deviceUpdate.Tags[0].Split(":")[1]);
                user.PushNotificationProvider = deviceType;
                await userManager.UpdateAsync(user);
                await _hub.CreateOrUpdateRegistrationAsync(registrationDescription);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<string> CreateRegistrationId()
        {
            return await _hub.CreateRegistrationIdAsync();
        }

        #region Pushing Notifications

        public void SendNotification(string message, EccoUser user)
        {
            if (user.PushNotificationProvider != 0 && user != null)
            {
                switch (user.PushNotificationProvider)
                {
                    case DeviceTypeConstants.IOS:
                        SendNotificationToApple(message, "username:" + user.UserName);
                        break;
                    case DeviceTypeConstants.ANDROID:
                        SendNotificationToAndroid(message, "username:" + user.UserName);
                        break;
                }
            }
        }

        private async void SendNotificationToApple(string message, string toUsername)
        {
            string payload = "{\"aps\":{\"alert\":\"" + message +"\"}}";
            await _hub.SendAppleNativeNotificationAsync(payload, toUsername);
        }

        private async void SendNotificationToAndroid(string message, string toUsername)
        {
            string payload = "{ \"data\":{ \"message\":\"" + message + "\"} }";
            await _hub.SendFcmNativeNotificationAsync(payload, toUsername);
        }

        #endregion
    }
}