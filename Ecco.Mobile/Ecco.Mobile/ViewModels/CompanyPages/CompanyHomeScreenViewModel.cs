using Ecco.Mobile.Views.Pages.CompanyPages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class CompanyHomeScreenViewModel : LoadingViewModel
    {
        private bool _isCompanyOwner;
        public bool IsCompanyOwner
        {
            get
            {
                return _isCompanyOwner;
            }
            set
            {
                _isCompanyOwner = value;
                OnPropertyChanged(nameof(IsCompanyOwner));
            }
        }

        private bool _hasEmployers;
        public bool HasEmployers
        {
            get
            {
                return _hasEmployers;
            }
            set
            {
                _hasEmployers = value;
                OnPropertyChanged(nameof(HasEmployers));
            }
        }

        public ICommand ViewMyCompanyCommand { get; set; }
        public ICommand ViewEmployeeHomeCommand { get; set; }
        public ICommand CreateCompanyCommand { get; set; }

        public CompanyHomeScreenViewModel() : base()
        {
            ViewMyCompanyCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new MyCompanyPage()));
            ViewEmployeeHomeCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new EmployeeHomePage()));
            CreateCompanyCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new CreateCompanyPage()));
        }

        protected async override void Load()
        {
            Loading = true;

            IsCompanyOwner = (await _db.GetMyOwnedCompany(_userData.Id)) != null;

            var employers = await _db.GetMyEmployers(_userData.Id.ToString());
            HasEmployers = employers.Count > 0;

            Loading = false;
        }
    }
}