﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Syncfusion.DataSource.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Connections
{
    public class PendingConnectionsViewModel : LoadingViewModel
    {
        public ObservableCollection<ConnectionModel> PendingConnections { get; set; } = new ObservableCollection<ConnectionModel>();

        public ICommand AcceptPendingConnectionCommand { get; set; }
        public ICommand DeletePendingConnectionCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        private Action _reloadConnections;

        public PendingConnectionsViewModel(Action reloadConnections) : base()
        {
            _reloadConnections = reloadConnections;
            AcceptPendingConnectionCommand = new Command<Connection>(AcceptPendingConnection);
            DeletePendingConnectionCommand = new Command<Connection>(DeletePendingConnection);
            RefreshCommand = new Command(LoadPendingConnections);
            LoadPendingConnections();
        }

        private async void LoadPendingConnections()
        {
            Loading = true;
            var pendingConnections = (await _db.GetMyPendingConnections(_userData.Id)).ToList();
            List<ConnectionModel> pendingConnectionModels = new List<ConnectionModel>();

            if (PendingConnections.Count != 0)
            {
                PendingConnections.Clear();
            }

            foreach (var pendingConnection in pendingConnections)
            {
                Entities.Card card = await _db.GetCard(pendingConnection.CardId);
                var userData = await _db.GetUserData(pendingConnection.FromId);
                var cardModel = CardModel.FromCard(card, await _db.GetUserData(card.UserId));

                ConnectionModel model = new ConnectionModel()
                {
                    Card = cardModel,
                    Connection = pendingConnection,
                    Name = userData.ProfileName
                };

                PendingConnections.Add(model);
            }

            Loading = false;
        }

        private async void AcceptPendingConnection(Connection connection)
        {
            var succesful = await _db.AcceptConnection(connection);
            if (succesful)
            {
                Console.WriteLine("Succesfully accepted connection request");
                LoadPendingConnections();
                _reloadConnections();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uh Oh!", "An error occured when attempting to accept this connection", "Ok");
            }
        }

        private async void DeletePendingConnection(Connection connection)
        {
            var succesful = await _db.DeletePendingConnection(connection);
            if (succesful)
            {
                LoadPendingConnections();
                _reloadConnections();
            }
            else
            { 
                await Application.Current.MainPage.DisplayAlert("Uh Oh!", "An error occured when attempting to delete this connection", "Ok");
            }
        }
    }
}