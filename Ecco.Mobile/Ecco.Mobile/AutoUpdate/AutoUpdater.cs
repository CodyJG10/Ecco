﻿using Ecco.Api;
using Ecco.Api.Events;
using Ecco.Entities;
using Ecco.Entities.Events;
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
        private readonly CloudEventListener eventListener;

        public const string CARDS = "Cards";
        public const string CONNECTIONS = "Connections";
        //public const string CONNECTION_INVITATION = "Connection Invitation";

        public AutoUpdater()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            string eventHubConnectionString = Properties.Resources.EventsHubConnectionString;
            string eventHubName = Properties.Resources.EventsHubName;
            string storageConnectionString = Properties.Resources.EventsHubStorageConnectionString;
            string storageContainerName = Properties.Resources.EventsHubStorageContainerName;

            eventListener = new CloudEventListener(eventHubConnectionString, eventHubName, storageContainerName, storageConnectionString);
        }

        public void Start()
        {
            CloudEventListener.OnCloudEventReceived += CloudEventListener_CloudEventReceivedEvent;
            eventListener.Start();
        }

        public void Stop()
        { 
            CloudEventListener.OnCloudEventReceived -= CloudEventListener_CloudEventReceivedEvent;
            eventListener.Stop();
        }

        private void CloudEventListener_CloudEventReceivedEvent(string msg)
        {
            try
            {
                var updateEvent = JsonConvert.DeserializeObject<UpdateEvent>(msg);
                if(updateEvent.User == _userData.UserName)
                {
                    UpdateUserConnections();
                    UpdateUserCard();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //private async void ReceiveUserConnections()
        //{
        //    var connections = await _db.GetMyConnections(_userData.Id);
        //    string json = JsonConvert.SerializeObject(connections);
        //    MessagingCenter.Send(this, CONNECTIONS, json);
        //}

        //private async void ReceiveUserPendingConnections()
        //{
        //    var connections = await _db.GetMyPendingConnections(_userData.Id);
        //    string json = JsonConvert.SerializeObject(connections);
        //    MessagingCenter.Send(this, CONNECTION_INVITATION, json);
        //}

        public async void UpdateUserCard()
        {
            var cards = await _db.GetMyCards(_userData.Id.ToString());
            string json = JsonConvert.SerializeObject(cards);
            MessagingCenter.Send(this, CARDS, json);
        }

        public async void UpdateUserConnections()
        {
            var connections = await _db.GetMyConnections(_userData.Id);
            string json = JsonConvert.SerializeObject(connections);
            MessagingCenter.Send(this, CONNECTIONS, json);
        }
    }
}