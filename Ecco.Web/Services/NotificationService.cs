using System;
using System.Collections.Generic;
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

        public async void SendNotification(string hubName, string connectionString, string notificationText)
        {
            Dictionary<string, string> templateParams = new Dictionary<string, string>();
            templateParams["messageParam"] = notificationText;

            try
            {
                var result = await _hub.SendTemplateNotificationAsync(templateParams);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
