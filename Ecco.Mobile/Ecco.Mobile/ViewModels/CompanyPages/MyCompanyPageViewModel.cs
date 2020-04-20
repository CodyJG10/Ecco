using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Pages.CompanyPages;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class MyCompanyPageViewModel : LoadingViewModel
    {
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

        public ICommand InviteEmployeeCommand { get; set; }

        public MyCompanyPageViewModel() : base()
        {
            InviteEmployeeCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new InviteEmployeeToCompanyPage()));
        }

        protected override async void Load()
        {
            Loading = true;
            
            var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            Company = await _db.GetMyOwnedCompany(userData.Id);

            TemplateImage = await TemplateUtil.LoadImageSource(Company, _db, _storage);
            
            Loading = false;
        }
    }
}