using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ICommand EditCardCommand { get; set; }
        public ICommand DeleteCardCommand { get; set; }

        private readonly AutoUpdater _autoUpdater;

        public ViewMyCardViewModel(CardModel model) : base()
        {
            CardModel = model;
            QrCodeUrl = "https://ecco-space.azurewebsites.net/cards/" + model.Card.Id.ToString();
            ShareCommand = new Command(Share);
            SetAsActiveCardCommand = new Command(SetAsActiveCard);
            EditCardCommand = new Command(EditCard);
            DeleteCardCommand = new Command(DeleteCard);
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

        private async void EditCard()
        {
            string serviceTitle = "";
            typeof(ServiceTypes).GetFields().ToList().ForEach(field => { if ((int)field.GetValue(null) == CardModel.Card.ServiceType) serviceTitle = (field.GetCustomAttributes(true)[0] as ServiceInfo).Title; });

            CreateCardModel model = new CreateCardModel()
            {
                CardTitle = CardModel.Card.CardTitle,
                Email = CardModel.Card.Email,
                PhoneNumber = CardModel.Card.Phone,
                ServiceCategory = serviceTitle,
                ExportedImageData = CardModel.Card.ExportedImageData,
                FullName = CardModel.Card.FullName,
                TemplateId = CardModel.Card.TemplateId,
                TemplateImage = await TemplateUtil.LoadImageSource(CardModel.Card.TemplateId, _db, _storage)
            };

            await Application.Current.MainPage.Navigation.PushAsync(new EditCardPage(model, CardModel.Card.TemplateId, CardModel.Card.Id));
        }

        private async void DeleteCard()
        {
            var succesful = await _db.DeleteCard(CardModel.Card);
            if (!succesful)
            {
                await Application.Current.MainPage.DisplayAlert("Error!", "An error was encountered when attempting to delete your card", "Ok");
            }
            (TinyIoCContainer.Current.Resolve<AutoUpdater>()).UpdateUserCard();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}