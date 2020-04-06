using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Views.Pages;
using Ecco.Mobile.Views.Pages.Cards;
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
    public class MyCardViewModel : ViewModelBase
    {
        private IDatabaseManager _db;

        #region Content

        private List<Entities.Card> _cards = new List<Entities.Card>();
        public List<Entities.Card> Cards
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

        #endregion

        #region Commands

        public ICommand CreateCardCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand EditCardCommand { get; set; }
        public ICommand DeleteCardCommand { get; set; }
        public ICommand SelectCardCommand { get; set; }

        #endregion

        public MyCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();

            CreateCardCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new CreateCardPage()));
            RefreshCommand = new Command(Refresh);
            EditCardCommand = new Command<Entities.Card>(EditCard);
            DeleteCardCommand = new Command<Entities.Card>(DeleteCard);
            SelectCardCommand = new Command<Entities.Card>(x => Application.Current.MainPage.Navigation.PushAsync(new ViewCardPage(x)));

            LoadCards();
        }

        private async void LoadCards()
        {
            Loading = true;

            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var cards = await _db.GetMyCards(user.Id.ToString());
            Cards = cards.ToList();

            Loading = false;
        }

        private async void Refresh()
        {
            Loading = true;

            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var cards = await _db.GetMyCards(user.Id.ToString());
            Cards = cards.ToList();

            Loading = false;
        }

        private async void EditCard(Entities.Card card)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new EditCardPage(card));
        }

        private async void DeleteCard(Entities.Card card)
        {
            var succesful = await _db.DeleteCard(card);
            if (succesful)
            {
                Refresh();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "An error was encountered when attempting to delete your card", "Ok");
                Refresh();
            }
        }
    }
}