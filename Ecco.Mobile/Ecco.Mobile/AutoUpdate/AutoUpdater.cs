using Ecco.Api;
using Ecco.Api.Events;
using Ecco.Entities;
using Ecco.Mobile.ViewModels.Home;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xamarin.Forms;

namespace Ecco.Mobile.AutoUpdate
{
    public class AutoUpdater
    {
        private readonly IDatabaseManager _db;
        private readonly UserData _userData;
        private readonly CloudEventListener listener;

        public const string CARDS = "Cards";
        public const string CONNECTIONS = "Connections";
        public const string CONNECTION_INVITATION = "Connection Invitation";

        public AutoUpdater()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            listener = new CloudEventListener()
            {
                EventHubConnectionString = "Endpoint=sb://ecco-space-events.servicebus.windows.net/;SharedAccessKeyName=main;SharedAccessKey=9b1iT0P8sq2MFjbPiFiNf9WTpbIia4/MmLh5jwvTOaM=;EntityPath=ecco-events",
                EventHubName = "ecco-events",
                StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=eccocloudstorage;AccountKey=sQD+1r+ZLTGWgFG70k2E7mZOLuX6pM1bAI94pjm2+tSUcnySJKP6rwwxs5rAQIksUivQkVpPv0lW7UyOorjx/g==;EndpointSuffix=core.windows.net",
                StorageContainerName = "events"
            };
            listener.InitClient();
        }

        public void Start()
        {
            CloudEventListener.OnCloudEventReceived += CloudEventListener_CloudEventReceivedEvent;
        }

        private void CloudEventListener_CloudEventReceivedEvent(string msg)
        {
            ReceiveUserConnections();
            ReceiveUserPendingConnections();
        }

        public void Stop()
        {
            CloudEventListener.OnCloudEventReceived -= CloudEventListener_CloudEventReceivedEvent;
        }

        private async void ReceiveUserConnections()
        {
            var connections = await _db.GetMyConnections(_userData.Id);
            string json = JsonConvert.SerializeObject(connections);
            MessagingCenter.Send(this, CONNECTIONS, json);
        }

        public async void UpdateUserCard() 
        {
            var cards = await _db.GetMyCards(_userData.Id.ToString());
            string json = JsonConvert.SerializeObject(cards);
            MessagingCenter.Send(this, CARDS, json);
        }

        private async void ReceiveUserPendingConnections()
        {
            var connections = await _db.GetMyPendingConnections(_userData.Id);
            string json = JsonConvert.SerializeObject(connections);
            MessagingCenter.Send(this, CONNECTION_INVITATION, json);
        }
    }
}