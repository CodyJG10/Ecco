using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Onboarding;
using Ecco.Mobile.Views.Pages.Cards;
using Ecco.Mobile.Views.Pages.Connections;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class CardListViewModel : ViewModelBase
    {
        private IDatabaseManager _db;
        private IStorageManager _storage;
        private UserData _user;

        #region Content

        private List<ConnectionModel> _cards;
        public List<ConnectionModel> Cards
        {
            get
            {
                return _cards;
            }
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }

        private bool _loading;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        private bool _hasPendingConnections;
        public bool HasPendingConnections
        {
            get
            {
                return _hasPendingConnections;
            }
            set
            {
                _hasPendingConnections = value;
                OnPropertyChanged(nameof(HasPendingConnections));
            }
        }

        private bool _hasConnections;
        public bool HasConnections
        {
            get 
            {
                return _hasConnections;
            }
            set
            {
                _hasConnections = value;
                OnPropertyChanged(nameof(HasConnections));
            }
        }

        #endregion

        public ICommand ViewPendingConnectionsCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand DeleteConnectionCommand { get; set; }
        public ICommand SelectCardCommand { get; set; }

        private bool _isAutoUpdating = false;

        public CardListViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();

            ViewPendingConnectionsCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new PendingConnectionsPage()));
            RefreshCommand = new Command(Refresh);
            DeleteConnectionCommand = new Command<ConnectionModel>(DeleteConnection);
            SelectCardCommand = new Command<CardModel>(x => Application.Current.MainPage.Navigation.PushAsync(new ViewCardPage(x)));

            SubscribeAutoUpdates();

            Load();
        }

        private async void Load()
        {
            Loading = true;
            await LoadPendingConnections();
            await LoadConnections();
            Loading = false;
        }

        public void SubscribeAutoUpdates()
        {
            if (_isAutoUpdating)
                return;
            MessagingCenter.Instance.Subscribe<AutoUpdater, string>(this, AutoUpdater.CONNECTIONS, (sender, json) =>
            {
                if (Loading) return;
                var connections = JsonConvert.DeserializeObject<List<Connection>>(json);
                if (Cards.Count != connections.Count)
                {
                    Refresh();
                }

            });

            MessagingCenter.Instance.Subscribe<AutoUpdater, string>(this, AutoUpdater.CONNECTION_INVITATION, (sender, json) =>
            {
                if (Loading) return;
                var connections = JsonConvert.DeserializeObject<List<Connection>>(json);
                if (HasPendingConnections == false && connections.Count > 0)
                {
                    Refresh();
                }
            });

            _isAutoUpdating = true;
        }

        public void UnsubscribeAutoUpdates() 
        {
            MessagingCenter.Instance.Unsubscribe<AutoUpdater>(this, AutoUpdater.CONNECTIONS);
            MessagingCenter.Instance.Unsubscribe<AutoUpdater>(this, AutoUpdater.CONNECTION_INVITATION);
            _isAutoUpdating = false;
        }

        #region Loading

        private async Task<Task> LoadConnections()
        {
            var connections = (await _db.GetMyConnections(_user.Id)).ToList();
            List<ConnectionModel> connectionModels = new List<ConnectionModel>();
            foreach (var connection in connections)
            {
                Entities.Card card = await _db.GetCard(connection.CardId);
                var userData = await _db.GetUserData(connection.FromId);
                var cardModel = CardModel.FromCard(card, await _db.GetUserData(card.UserId));


                ConnectionModel model = new ConnectionModel()
                {
                    Card = cardModel,
                    Connection = connection,
                    Name = userData.ProfileName
                };
                connectionModels.Add(model);
            }

            HasConnections = connectionModels.Count > 0;

            Cards = connectionModels;
            return Task.CompletedTask;
        }

        private async Task<Task> LoadPendingConnections()
        {
            var pendingConnections = (await _db.GetMyPendingConnections(_user.Id));
            HasPendingConnections = pendingConnections.Count() > 0;
            return Task.CompletedTask;
        }

        #endregion

        public void Refresh()
        {
            Load();
        }

        private async void DeleteConnection(ConnectionModel model)
        {
            var succesful = await _db.DeleteConnection(model.Connection);
            if (succesful)
            {
                Refresh();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "There was an error attempting to delete this connection!", "Ok");
                Refresh();
            }
        }
    }
}