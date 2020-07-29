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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public ObservableCollection<object> Cards { get; set; } =  new ObservableCollection<object>();
        
        private CardModel _activeCard;
        public CardModel ActiveCard 
        {
            get
            {
                return _activeCard;
            }
            set
            {
                _activeCard = value;
                OnPropertyChanged(nameof(ActiveCard));
            }
        }
     
        private bool _hasActiveCard;
        public bool HasActiveCard
        {
            get
            {
                return _hasActiveCard;
            }
            set
            {
                _hasActiveCard = value;
                OnPropertyChanged(nameof(HasActiveCard));
            }
        }

        private CardModel _selectedCard;
        public CardModel SelectedCard
        {
            get 
            {
                return _selectedCard;
            }
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        private string _sharedWithText;
        public string SharedWithText
        {
            get
            {
                return _sharedWithText;
            }
            set 
            {
                _sharedWithText = value;
                OnPropertyChanged(nameof(SharedWithText));
            }
        }

        private bool _isCreateCard = false;
        public bool IsCreateCard 
        {
            get
            {
                return _isCreateCard;
            }
            set
            {
                _isCreateCard = value;
                OnPropertyChanged(nameof(IsCreateCard));
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
            Loading = true;

            CreateCardCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new CreateCardPage()));
            RefreshCommand = new Command(LoadCards);
            EditCardCommand = new Command<CardModel>(EditCard);
            DeleteCardCommand = new Command<CardModel>(DeleteCard);
            SelectCardCommand = new Command<CardModel>(SelectCard);
            ShareCardCommand = new Command<CardModel>(ShareCard);

            SubscribeAutoUpdates();

            LoadCards();
        }

        public void SubscribeAutoUpdates()
        {
            MessagingCenter.Instance.Subscribe<AutoUpdater, string>(this, AutoUpdater.CARDS, (sender, json) =>
            {
                if (Loading) return;
                var cards = JsonConvert.DeserializeObject<List<Entities.Card>>(json);
                LoadCards();
            });
        }

        private async void SelectCard(CardModel card)
        {
            if (Loading)
                return;

            if (card == null)
                card = SelectedCard;

            await Application.Current.MainPage.Navigation.PushAsync(new MyCard(card));
        }

        private async void LoadCards()
        {
            Loading = true;

            //Load all cards
            if (Cards != null) Cards.Clear();

            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var cards = (await _db.GetMyCards(user.Id.ToString())).ToList();

            var activeCard = await _db.GetActiveCard(_userData.Id.ToString());

            Cards.Add(null);

            foreach (var card in cards)
            {
                var cardModel = CardModel.FromCard(card, _userData);
                cardModel.IsActiveCard = activeCard.Id == card.Id;
                Cards.Add(cardModel);
            }

            //Load active card data
            if (activeCard != null)
            {
                ActiveCard = CardModel.FromCard(activeCard, _userData);
                HasActiveCard = true;
            }
            else
            {
                HasActiveCard = false;
            }

            if (SelectedCard == null)
            {
                if (Cards.Count > 1)
                {
                    ShowCardInfo((Cards[Cards.Count - 1] as CardModel));
                }
            }

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
            if (!succesful)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "An error was encountered when attempting to delete your card", "Ok");
            }
            LoadCards();
        }

        private async void ShareCard(CardModel card)
        {
            await Share.RequestAsync(new ShareTextRequest()
            {
                Uri = "https://ecco-space.azurewebsites.net/" + card.Card.Id,
                Text = "Hey, check out my business card on Ecco Space!",
                Title = "Digital Business Card"
            });
        }

        public async void ShowCardInfo(CardModel card)
        {
            SelectedCard = card;

            if (card == null)
            {
                IsCreateCard = true;
                return;
            }
            else if(IsCreateCard)
            {
                IsCreateCard = false;
            }

            var sharedWithCount = await _db.GetConnectionsWithCard(card.Card);
            if (sharedWithCount == 0)
            {
                SharedWithText = "shared with nobody";
            }
            else if (sharedWithCount == 1)
            {
                SharedWithText = "shared with 1 person";
            }
            else
            {
                SharedWithText = "shared with " + sharedWithCount + " people";
            }
        }
    }
}