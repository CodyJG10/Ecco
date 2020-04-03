using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
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

        private List<ConnectionModel> _pendingConnections;
        public List<ConnectionModel> PendingConnections
        {
            get
            {
                return _pendingConnections;
            }
            set
            {
                _pendingConnections = value;
                OnPropertyChanged(nameof(PendingConnections));
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

        #endregion

        #region Commands
        
        public ICommand AcceptPendingConnectionCommand { get; set; }
        
        #endregion

        public CardListViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            
            Load();

            AcceptPendingConnectionCommand = new Command<Connection>(AcceptPendingConnection);
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
            Cards = connectionModels;
            return Task.CompletedTask;
        }

        private async Task<Task> LoadPendingConnections()
        {
            var pendingConnections = (await _db.GetMyPendingConnections(_user.Id)).ToList();
            List<ConnectionModel> pendingConnectionModels = new List<ConnectionModel>(); 
            foreach (var pendingConnection in pendingConnections)
            {
                Card card = await _db.GetCard(pendingConnection.CardId);
                string userName = (await _db.GetUserData(pendingConnection.FromId)).UserName;
                ConnectionModel model = new ConnectionModel()
                {
                    Card = card,
                    Connection = pendingConnection,
                    Name = userName
                };
                pendingConnectionModels.Add(model);
            }
            PendingConnections = pendingConnectionModels;
            return Task.CompletedTask;
        }

        #endregion

        private async void AcceptPendingConnection(Connection connection)
        {
            var succesful = await _db.AcceptConnection(connection);
            if (succesful)
            {
                Console.WriteLine("Succesfully accepted connection request");
                Load();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uh Oh!", "An error occured when attempting to accept this connection", "Ok");
            }
        }
    }
}