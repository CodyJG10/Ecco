using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.AutoUpdate;
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
            SelectCardCommand = new Command<CardModel>(x => Application.Current.MainPage.Navigation.PushAsync(new MyCard(x)));

            LoadCards();

            InitAutoUpdate();
        }

        private void InitAutoUpdate()
        {
            MessagingCenter.Instance.Subscribe<AutoUpdater, string>(this, AutoUpdater.CARDS, (sender, json) =>
            {
                if (Loading) return;
                var cards = JsonConvert.DeserializeObject<List<Entities.Card>>(json);
                if (Cards.Count != cards.Count)
                {
                    LoadCards();
                }
            });
        }

        private async void LoadCards()
        {
            Loading = true;

            if (Cards != null) Cards.Clear();

            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var cards = (await _db.GetMyCards(user.Id.ToString())).ToList();

            List<CardModel> cardModels = new List<CardModel>();

            foreach (var card in cards)
            {
                CardModel model = CardModel.FromCard(card);
                cardModels.Add(model);
            }

            Cards = cardModels;

            Loading = false;
        }

        private async void EditCard(CardModel card)
        {

            string serviceTitle = "";
            typeof(ServiceTypes).GetFields().ToList().ForEach(field => { if ((int)field.GetValue(null) == card.Card.ServiceType) serviceTitle = (field.GetCustomAttributes(true)[0] as ServiceInfo).Title; });


            CreateCardModel model = new CreateCardModel()
            {
                CardTitle = card.Card.CardTitle,
                Email = card.Card.Email,
                PhoneNumber = card.Card.Phone,
                ServiceCategory = serviceTitle
            };

            await Application.Current.MainPage.Navigation.PushAsync(new EditCardPage(model, card.Card.TemplateId, card.Card.Id));
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