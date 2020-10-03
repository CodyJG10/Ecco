using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Nancy.TinyIoc;
using System.Linq;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class CreateCardEditorViewModel : LoadingViewModel
    {
        public CreateCardModel Model { get; set; }

        private bool _userHasCards;

        public CreateCardEditorViewModel(CreateCardModel model) : base()
        {
            Model = model;
            LoadUserHasCards();
        }

        private async void LoadUserHasCards()
        { 
            var allCards = (await _db.GetMyCards(_userData.Id.ToString())).ToList();
            _userHasCards = allCards.Count == 0;
        }

        public async void CreateCard()
        {
            Entities.Card card = new Entities.Card()
            {
                CardTitle = Model.CardTitle,
                Email = Model.Email,
                Phone = Model.PhoneNumber,
                ExportedImageData = Model.ExportedImageData,
                UserId = _userData.Id,
                TemplateId = Model.TemplateId,
                FullName = Model.FullName,
                Address = Model.Address,
                Website = Model.Website
            };

            var fields = typeof(ServiceTypes).GetFields();
            int serviceTypeId = 1;
            foreach (var field in fields)
            {
                var serviceInfo = field.GetCustomAttributes(true)[0] as ServiceInfo;
                string title = serviceInfo.Title;
                if (title.Equals(Model.ServiceCategory))
                {
                    int id = (int)field.GetValue(null);
                    serviceTypeId = id;
                    break;
                }
            }

            card.ServiceType = serviceTypeId;

            var succeeded = await _db.CreateCard(card);
            if (succeeded)
            {
                if (_userHasCards)
                {
                    var allCards = (await _db.GetMyCards(_userData.Id.ToString())).ToList();
                    var newCard = allCards[0];
                    await _db.UpdateActiveCard(_userData.Id.ToString(), newCard.Id.ToString());
                }
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopAsync();
                TinyIoCContainer.Current.Resolve<AutoUpdater>().UpdateUserCard();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card.", "Ok");
                Loading = false;
            }
        }
    }
}