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

namespace Ecco.Mobile.ViewModels.Home.Connections
{
    public class PendingConnectionsViewModel : ViewModelBase
    {
        private IDatabaseManager _db;
        private UserData _user;

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

        public ICommand AcceptPendingConnectionCommand { get; set; }

        public PendingConnectionsViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            AcceptPendingConnectionCommand = new Command<Connection>(AcceptPendingConnection);

            LoadPendingConnections();
        }

        private async void LoadPendingConnections()
        {
            Loading = true;
            var pendingConnections = (await _db.GetMyPendingConnections(_user.Id)).ToList();
            List<ConnectionModel> pendingConnectionModels = new List<ConnectionModel>();
            foreach (var pendingConnection in pendingConnections)
            {
                Entities.Card card = await _db.GetCard(pendingConnection.CardId);
                var userData = await _db.GetUserData(pendingConnection.FromId);
                ConnectionModel model = new ConnectionModel()
                {
                    Card = card,
                    Connection = pendingConnection,
                    Name = userData.ProfileName
                };
                pendingConnectionModels.Add(model);
            }
            PendingConnections = pendingConnectionModels;
            Loading = false;
        }

        private async void AcceptPendingConnection(Connection connection)
        {
            var succesful = await _db.AcceptConnection(connection);
            if (succesful)
            {
                Console.WriteLine("Succesfully accepted connection request");
                LoadPendingConnections();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uh Oh!", "An error occured when attempting to accept this connection", "Ok");
            }
        }
    }
}