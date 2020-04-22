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

        //public async Task<Task> RegisterDevice(DeviceRegistration deviceRegistration, ApplicationDbContext context, UserManager<EccoUser> userManager)
        //{


        //    Installation installation = new Installation
        //    {
        //        InstallationId = deviceRegistration.InstallationId,
        //        PushChannel = deviceRegistration.PushChannel,
        //        Tags = deviceRegistration.Tags,
        //    };

        //    switch (deviceRegistration.Platform)
        //    {
        //        case "apns":
        //            installation.Platform = NotificationPlatform.Apns;
        //            break;
        //        case "fcm":
        //            installation.Platform = NotificationPlatform.Fcm;
        //            break;
        //    }

        //    await _hub.CreateOrUpdateInstallationAsync(installation);

        //    var user = await userManager.FindByNameAsync(deviceRegistration.Username);
        //    user.DeviceInstallationId = deviceRegistration.InstallationId;
        //    context.Update(user);
        //    await context.SaveChangesAsync();

        //    return Task.CompletedTask;
        //}

        public async Task<string> Post(string handle = null)
        {
            string newRegistrationId = null;

            if (handle != null)
            {
                var registrations = await _hub.GetRegistrationsByChannelAsync(handle, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await _hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
                newRegistrationId = await _hub.CreateRegistrationIdAsync();

            return newRegistrationId;
        }

        public async Task<DeviceRegistration> GetDeviceRegistration(string userName, UserManager<EccoUser> userManager)
        {
            var user = await userManager.FindByNameAsync(userName);
            var installation = await _hub.GetInstallationAsync(user.DeviceInstallationId);
            DeviceRegistration registration = new DeviceRegistration()
            {
                InstallationId = user.DeviceInstallationId,
                Platform = installation.Platform.ToString(),
                PushChannel = installation.PushChannel,
                Tags = installation.Tags.ToArray()
            };
            return registration; 
        }

        public void SendNotification(string message, int deviceType, string userName)
        {
            switch (deviceType)
            {
                case DeviceTypeConstants.IOS:
                    SendNotificationToApple(message, userName);
                    break;
                case DeviceTypeConstants.ANDROID:
                    SendNotificationToAndroid(message, userName);
                    break;
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
    }
}
