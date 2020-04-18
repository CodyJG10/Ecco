using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecco.Entities.Constants;
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

        public void SendNotification(string message, int deviceType)
        {
            switch (deviceType)
            {
                case DeviceTypeConstants.IOS:
                    SendNotificationToApple(message);
                    break;
                case DeviceTypeConstants.ANDROID:
                    SendNotificationToAndroid(message);
                    break;
            }
        }

        private async void SendNotificationToApple(string message)
        {
            string payload = "{\"aps\":{\"alert\":\"" + message +"\"}}";
            await _hub.SendAppleNativeNotificationAsync(payload);
        }

        private async void SendNotificationToAndroid(string message)
        {
            string payload = "{ \"data\":{ \"message\":\"" + message + "\"} }";
            await _hub.SendFcmNativeNotificationAsync(payload);
        }
    }
}
