using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
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

        public CardListViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            ViewPendingConnectionsCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new PendingConnectionsPage()));
            RefreshCommand = new Command(Refresh);

            Load();
        }

        private async void Load()
        {
            Loading = true;
            await LoadPendingConnections();
            await LoadConnections();
            Loading = false;
        }

        #region Loading

        private async Task<Task> LoadConnections()
        {
            var connections = (await _db.GetMyConnections(_user.Id)).ToList();
            List<ConnectionModel> connectionModels = new List<ConnectionModel>();
            foreach (var connection in connections)
            {
                Card card = await _db.GetCard(connection.CardId);
                string userName = (await _db.GetUserData(connection.FromId)).UserName;
                ConnectionModel model = new ConnectionModel()
                {
                    Card = card,
                    Connection = connection,
                    Name = userName
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
    }
}