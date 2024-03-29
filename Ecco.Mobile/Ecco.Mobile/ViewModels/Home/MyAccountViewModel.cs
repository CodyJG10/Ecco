﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Views;
using Ecco.Mobile.Views.Authentication;
using Ecco.Mobile.Views.Pages.CompanyPages;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class MyAccountViewModel : ViewModelBase
    {
        private IDatabaseManager _db;
        
        private UserData _userData;
        public UserData UserData
        {
            get
            {
                return _userData;
            }
            set
            {
                _userData = value;
                OnPropertyChanged(nameof(UserData));
            }
        }

        public ICommand LogoutCommand { get; set; }
        public ICommand MyCompanyCommand { get; set; }

        public MyAccountViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            UserData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            LogoutCommand = new Command(Logout);
            MyCompanyCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new CompanyHomeScreen()));
        }

        private void Logout()
        {
            //CrossSettings.Current.Clear();
            CrossSettings.Current.Remove("UserData");
            CrossSettings.Current.Remove("Username");
            CrossSettings.Current.Remove("Password");
            CrossSettings.Current.Remove("RefreshToken");
            (TinyIoCContainer.Current.Resolve<AutoUpdater>()).Stop();
            TinyIoCContainer.Current.Unregister(typeof(AutoUpdater));
            Application.Current.MainPage = new LoginPage();
        }
    }
}