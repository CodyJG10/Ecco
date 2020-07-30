using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views.NFC;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class ViewMyCardViewModel : LoadingViewModel
    {
        public CardModel CardModel { get; set; }
        public string QrCodeUrl { get; set; }

        private bool _isActiveCard;
        public bool IsActiveCard
        {
            get
            {
                return _isActiveCard;
            }
            set
            {
                _isActiveCard = value;
                OnPropertyChanged(nameof(IsActiveCard));
            }
        }

        public ICommand ShareCommand { get; set; }
        public ICommand SetAsActiveCardCommand { get; set; }

        private AutoUpdater _autoUpdater;

        public ViewMyCardViewModel(CardModel model) : base()
        {
            CardModel = model;
            QrCodeUrl = "https://ecco-space.azurewebsites.net/cards/" + model.Card.Id.ToString();
            ShareCommand = new Command(Share);
            SetAsActiveCardCommand = new Command(SetAsActiveCard);
            CheckIfActiveCard();
            _autoUpdater = TinyIoCContainer.Current.Resolve<AutoUpdater>();
        }

        private async void CheckIfActiveCard()
        {
            var activeCard = await _db.GetActiveCard(_userData.Id.ToString());
            if (activeCard == null)
                IsActiveCard = false;
            else
                IsActiveCard = CardModel.Card.Id == activeCard.Id;
        }

        public async void Share()
        {
            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest()
            {
                Uri = QrCodeUrl,
                Text = "Hey, check out my digital business card on Ecco Space!",
                Title = "Digital Business Card"
            });
        }

        public async void SetAsActiveCard()
        {
            var result = await _db.UpdateActiveCard(_userData.Id.ToString(), CardModel.Card.Id.ToString());
            if (result.IsSuccessStatusCode)
            { 
                IsActiveCard = true;
                //await Application.Current.MainPage.Navigation.PopAsync();
                _autoUpdater.UpdateUserCard();
            }
        }
    }
}