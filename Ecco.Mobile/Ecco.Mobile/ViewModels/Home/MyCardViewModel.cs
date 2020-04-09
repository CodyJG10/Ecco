using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
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
        private IStorageManager _storage;

        #region Content

        private List<CardModel> _cards = new List<CardModel>();
        public List<CardModel> Cards
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
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();

            CreateCardCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new CreateCardPage()));
            RefreshCommand = new Command(LoadCards);
            EditCardCommand = new Command<CardModel>(EditCard);
            DeleteCardCommand = new Command<CardModel>(DeleteCard);
            SelectCardCommand = new Command<CardModel>(x => Application.Current.MainPage.Navigation.PushAsync(new ViewCardPage(x)));

            LoadCards();
        }

        private async void LoadCards()
        {
            Loading = true;

            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var cards = (await _db.GetMyCards(user.Id.ToString())).ToList();

            List<CardModel> cardModels = new List<CardModel>();

            foreach (var card in cards)
            {
                var templateImage = await TemplateUtil.LoadImageSource(card, _db, _storage);
                CardModel model = new CardModel()
                {
                    Card = card,
                    TemplateImage = templateImage
                };
                cardModels.Add(model);
            }

            Cards = cardModels;

            Loading = false;
        }

        private async void EditCard(CardModel card)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new EditCardPage(card));
        }

        private async void DeleteCard(CardModel card)
        {
            var succesful = await _db.DeleteCard(card.Card);
            if (succesful)
            {
                LoadCards();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "An error was encountered when attempting to delete your card", "Ok");
                LoadCards();
            }
        }
    }
}