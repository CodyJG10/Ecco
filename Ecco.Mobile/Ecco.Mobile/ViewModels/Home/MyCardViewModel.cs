﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Views.Pages;
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

        #region Content

        private List<Card> _cards;
        public List<Card> Cards
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

        #endregion

        public MyCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();

            CreateCardCommand = new Command(() => Application.Current.MainPage.Navigation.PushAsync(new CreateCardPage()));

            Loading = true;
            LoadCards();
        }

        private async void LoadCards()
        {
            var user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            var allCards = await _db.GetCards();
            //var allConnections = await _db.GetConnections(user.Id);

            var userCards = allCards.Where(x => x.UserId == user.Id);

            Loading = false;
        }
    }
}