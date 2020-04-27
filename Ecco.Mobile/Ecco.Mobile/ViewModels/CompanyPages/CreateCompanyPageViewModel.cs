using Ecco.Entities.Company;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class CreateCompanyPageViewModel : LoadingViewModel
    {
        public string CompanyName { get; set; }

        public ICommand CreateCommand { get; set; }

        public CreateCompanyPageViewModel() : base()
        {
            CreateCommand = new Command(Create);
        }

        private async void Create()
        {
            Loading = true;
            
            Company company = new Company()
            {
                CompanyName = CompanyName,
                OwnerId = _userData.Id
            };

            var succesful = await _db.CreateCompany(company);

            if (succesful)
            {
                await Application.Current.MainPage.DisplayAlert("Company Created", "Your company has succesfuly been created! Please check your email for further setup information.", "Ok");
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when trying to create your company", "Ok");
            }

            Loading = false;
        }
    }
}