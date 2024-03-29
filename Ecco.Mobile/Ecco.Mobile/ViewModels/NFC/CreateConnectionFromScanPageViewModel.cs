﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Constants;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.NFC
{
    public class CreateConnectionFromScanPageViewModel : ViewModelBase
    {
        private readonly IDatabaseManager _db;
        private readonly UserData _userData;

        public CardModel Model { get; set; }

        public ICommand AddConnectionCommand { get; set; }

        public CreateConnectionFromScanPageViewModel(CardModel model)
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>() as IDatabaseManager;
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            Model = model;

            AddConnectionCommand = new Command(AddConnection);
        }

        private async void AddConnection()
        {
            Connection connection = new Connection()
            {
                FromId = Model.Card.UserId,
                Status = ConnectionConstants.COMPLETE,
                ToId = _userData.Id,
                CardId = Model.Card.Id
            };

            var success = await _db.CreateConnectionAndAccept(connection);
            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success!", "Connection succesfully saved", "Great");
                TinyIoCContainer.Current.Resolve<AutoUpdater>().UpdateUserConnections();
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            { 
                await Application.Current.MainPage.DisplayAlert("Error", "There was an error creating the connection", "Ok");
            }
        }
    }
}