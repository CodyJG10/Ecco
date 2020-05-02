using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Onboarding;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class MyCardViewModel : LoadingViewModel
    {
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

        #endregion

        #region Commands

        public ICommand CreateCardCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        public ICommand EditCardCommand { get; set; }
        public ICommand DeleteCardCommand { get; set; }
        public ICommand SelectCardCommand { get; set; }
        public ICommand ShareCardCommand { get; set; }

        #endregion

        public MyCardViewModel() : base()
        {
            CreateCardCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new CreateCardPage()));
            RefreshCommand = new Command(LoadCards);
            EditCardCommand = new Command<CardModel>(EditCard);
            DeleteCardCommand = new Command<CardModel>(DeleteCard);
            SelectCardCommand = new Command<CardModel>(x => Application.Current.MainPage.Navigation.PushAsync(new MyCard(x)));
            ShareCardCommand = new Command<CardModel>(ShareCard);

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
                var cardModel = CardModel.FromCard(card, _userData);
                cardModels.Add(cardModel);
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
                ServiceCategory = serviceTitle,
                ExportedImageData = card.Card.ExportedImageData,
                FullName = card.Card.FullName,
                TemplateId = card.Card.TemplateId,
                TemplateImage = await TemplateUtil.LoadImageSource(card.Card.TemplateId, _db, _storage)
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

        private async void ShareCard(CardModel card)
        {
            await Share.RequestAsync(new ShareTextRequest()
            {
                Uri = "https://ecco-space.azurewebsites.net/" + card.Card.Id,
                Text = "Hey, check out my digital business card on Ecco Space!",
                Title = "Digital Business Card"
            });
        }
    }
}