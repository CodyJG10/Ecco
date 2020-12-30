using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Entities.Constants;
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
    public class InviteEmployeeToCompanyPageViewModel : LoadingViewModel
    {
        public string ToUsername { get; set; }

        public ICommand SendCommand { get; set; }

        public InviteEmployeeToCompanyPageViewModel() : base()
        {
            SendCommand = new Command(Send);
        }

        private async void Send()
        {
            Loading = true;

            var company = await _db.GetMyOwnedCompany(_userData.Id);

            try
            {
                var toUser = await _db.GetUserData(ToUsername);

                EmployeeInvitation invitation = new EmployeeInvitation()
                {
                    CompanyId = company.Id,
                    Status = ConnectionConstants.PENDING,
                    UserId = toUser.Id
                };

                var succesful = await _db.InviteEmployee(invitation);
                if (succesful)
                {
                    await Application.Current.MainPage.DisplayAlert("Invitation Sent", "Succesfully sent employee invitation to: " + ToUsername, "Return");
                }
                else
                { 
                    await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when trying to invite the user", "Return");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Could not find user with email: " + ToUsername, "Return");
            }

            Loading = false;
        }
    }
}