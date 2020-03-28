﻿using Ecco.Api;
using Ecco.Entities;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class CardListViewModel : ViewModelBase
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

        public CardListViewModel()
        { 
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            
            Loading = true;
           // LoadCards();
        }
    }
}