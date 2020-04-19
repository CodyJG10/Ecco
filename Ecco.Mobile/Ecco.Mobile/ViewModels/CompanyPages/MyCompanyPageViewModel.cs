using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class MyCompanyPageViewModel : ViewModelBase
    {
        #region Fields
        private IDatabaseManager _db;
        private IStorageManager _storage;
        #endregion

        private Company _company;
        public Company Company
        {
            get
            {
                return _company;
            }
            set
            {
                _company = value;
                OnPropertyChanged(nameof(Company));
            }
        }

        private ImageSource _templateImage { get; set; }
        public ImageSource TemplateImage
        {
            get
            {
                return _templateImage;
            }
            set
            {
                _templateImage = value;
                OnPropertyChanged(nameof(TemplateImage));
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

        public MyCompanyPageViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            Load();
        }

        private async void Load()
        {
            Loading = true;
            
            var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            Company = await _db.GetMyOwnedCompany(userData.Id);

            TemplateImage = await TemplateUtil.LoadImageSource(Company, _db, _storage);
            
            Loading = false;
        }
    }
}