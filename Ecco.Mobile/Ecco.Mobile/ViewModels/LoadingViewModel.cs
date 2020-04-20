using Ecco.Api;
using Ecco.Entities;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.ViewModels
{
    public class LoadingViewModel : ViewModelBase
    {
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
        
        protected IDatabaseManager _db;
        protected IStorageManager _storage;
        protected UserData _userData;

        public LoadingViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            Load();
        }

        protected virtual void Load() { }
    }
}