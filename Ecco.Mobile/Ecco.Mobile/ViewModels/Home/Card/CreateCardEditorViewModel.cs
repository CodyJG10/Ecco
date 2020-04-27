using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class CreateCardEditorViewModel : LoadingViewModel
    {
        private IDatabaseManager _db;
        private UserData _userData;

        public CreateCardModel Model { get; set; }

        public CreateCardEditorViewModel(CreateCardModel model)
        {
            Model = model;
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
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
                FullName = Model.FullName
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
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card.", "Ok");
                Loading = false;
            }
        }
    }
}