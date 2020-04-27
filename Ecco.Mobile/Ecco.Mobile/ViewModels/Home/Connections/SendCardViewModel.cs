using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class SendCardViewModel : ViewModelBase
    {
        private IDatabaseManager _db;
        private IStorageManager _storage;
        private UserData _user;
        
        private string toId;
        private int cardToSendId;

        #region Properties
        public string UserQuery { get; set; }
        public ObservableCollection<UserData> UserResults { get; set; } = new ObservableCollection<UserData>();
        public ObservableCollection<CardModel> MyCards { get; set; } = new ObservableCollection<CardModel>();

        private bool _loading = true;
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
        public ICommand SendCommand { get; set; }
        public ICommand UserSearchTypedCommand { get; set; }
        public ICommand UserSelectedCommand { get; set; }
        public ICommand CardSelectedCommand { get; set; }
        #endregion

        public SendCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            SendCommand = new Command(Send);
            UserSearchTypedCommand = new Command(UpdateUserSearchResults);
            UserSelectedCommand = new Command<UserData>(UserSelected);
            CardSelectedCommand = new Command<CardModel>(CardSelected);

            LoadCards();
        }

        private async void LoadCards()
        {
            var cards = (await _db.GetMyCards(_user.Id.ToString())).ToList();

            foreach (var card in cards)
            {
                var model = CardModel.FromCard(card, _user);
                MyCards.Add(model);
            }

            Loading = false;
        }

        private async void Send()
        {

            if (cardToSendId == 0)
            { 
                await Application.Current.MainPage.DisplayAlert("Issue Sending", "Please select what card you would like to send", "Ok");
                return;
            }

            if (toId == null || toId.Length == 0)
            { 
                await Application.Current.MainPage.DisplayAlert("Issue Sending", "Please select who you would like to send your card to", "Ok");
                return;
            }

            bool succesful = await _db.CreateConnection(_user.Id, new Guid(toId), cardToSendId);
            if (succesful)
            {
                Console.WriteLine("Succesfully sent connection request");
                await Application.Current.MainPage.DisplayAlert("Good News", "Your card has succesfully been sent!", "Great!");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Uh Oh!", "An error occured when attempting to send the connection request", "Ok");
            }
        }

        private async void UpdateUserSearchResults()
        {
            UserResults.Clear();
            bool userExists = await _db.UserExists(UserQuery);
            if (userExists && !UserQuery.ToLower().Equals(_user.ProfileName.ToLower()))
            {
                var userData = await _db.GetUserData(UserQuery);
                UserResults.Add(userData);
            }
        }

        private void UserSelected(UserData userData)
        {
            toId = userData.Id.ToString();
        }

        private void CardSelected(CardModel card)
        {
            cardToSendId = card.Card.Id;
        }
    }
}