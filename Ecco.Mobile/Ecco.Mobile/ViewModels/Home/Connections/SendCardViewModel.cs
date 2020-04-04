using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
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
        private UserData _user;
        
        private string toId;
        private int cardToSendId;

        #region Properties
        public string UserQuery { get; set; }

        private List<UserData> _userResults;
        public List<UserData> UserResults
        {
            get
            {
                return _userResults;
            }
            set
            {
                _userResults = value;
                OnPropertyChanged(nameof(UserResults));
            }
        }

        private List<Entities.Card> _myCards;
        public List<Entities.Card> MyCards
        {
            get 
            {
                return _myCards;
            }
            set
            {
                _myCards = value;
                OnPropertyChanged(nameof(MyCards));
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
            MyCards = new List<Entities.Card>();
            
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            SendCommand = new Command(Send);
            UserSearchTypedCommand = new Command(UpdateUserSearchResults);
            UserSelectedCommand = new Command<UserData>(UserSelected);
            CardSelectedCommand = new Command<Entities.Card>(CardSelected);

            LoadCards();
        }

        private async void LoadCards()
        {
            var cards = await _db.GetMyCards(_user.Id.ToString());
            MyCards = cards.ToList();
        }

        private async void Send()
        {
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
            List<UserData> newResults = new List<UserData>();
            bool userExists = await _db.UserExists(UserQuery);
            if (userExists && !UserQuery.ToLower().Equals(_user.ProfileName.ToLower()))
            {
                var userData = await _db.GetUserData(UserQuery);
                newResults.Add(userData);
            }
            UserResults = newResults;
        }

        private void UserSelected(UserData userData)
        {
            toId = userData.Id.ToString();
        }

        private void CardSelected(Entities.Card card)
        {
            cardToSendId = card.Id;
        }
    }
}