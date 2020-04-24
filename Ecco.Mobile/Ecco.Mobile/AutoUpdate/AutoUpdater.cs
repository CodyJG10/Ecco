using Ecco.Api;
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
        private Thread thread;
        private UserData _userData;

        private bool running = true;
        public const int delay = 3000;

        public const string CARDS = "Cards";
        public const string CONNECTIONS = "Connections";
        public const string CONNECTION_INVITATION = "Connection Invitation";

        public AutoUpdater()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
        }

        public void Start()
        {
            ThreadStart threadStart = new ThreadStart(() =>
            {
                CheckForUpdates();
            });
            thread = new Thread(threadStart);
            thread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        private void CheckForUpdates()
        {
            while (running)
            {
                Thread.Sleep(delay);
                RecieveUserCards();
                RecieveUserConnections();
                RecieveUserPendingConnections();
            }
        }

        private async void RecieveUserCards()
        {
            var cards = await _db.GetMyCards(_userData.Id.ToString());
            string json = JsonConvert.SerializeObject(cards);
            MessagingCenter.Send(this, CARDS, json);
        }

        private async void RecieveUserConnections()
        {
            var connections = await _db.GetMyConnections(_userData.Id);
            string json = JsonConvert.SerializeObject(connections);
            MessagingCenter.Send(this, CONNECTIONS, json);
        }

        private async void RecieveUserPendingConnections()
        {
            var connections = await _db.GetMyPendingConnections(_userData.Id);
            string json = JsonConvert.SerializeObject(connections);
            MessagingCenter.Send(this, CONNECTION_INVITATION, json);
        }
    }
}