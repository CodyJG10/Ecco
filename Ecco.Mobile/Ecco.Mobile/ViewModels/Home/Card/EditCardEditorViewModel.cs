﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.AutoUpdate;
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
    public class EditCardEditorViewModel : LoadingViewModel
    {
        public CreateCardModel Model { get; set; }

        public EditCardEditorViewModel(CreateCardModel model) : base()
        {
            Model = model;
        }

        public async void SaveCard()
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
                Id = Model.id,
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

            var succeeded = await _db.EditCard(card);
            if (succeeded)
            {
                TinyIoCContainer.Current.Resolve<AutoUpdater>().UpdateUserCard();
                await Application.Current.MainPage.Navigation.PopToRootAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card.", "Ok");
                Loading = false;
            }
        }
    }
}
