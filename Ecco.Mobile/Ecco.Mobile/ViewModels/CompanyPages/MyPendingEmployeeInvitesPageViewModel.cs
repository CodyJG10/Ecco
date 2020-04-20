using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Company;
using Ecco.Mobile.Models;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class MyPendingEmployeeInvitesPageViewModel : LoadingViewModel
    {
        public ObservableCollection<EmployeeInvitationAndCompanyModel> MyEmployeeInvitations { get; set; } = new ObservableCollection<EmployeeInvitationAndCompanyModel>();

        public ICommand InvitationAcceptedCommand { get; set; }
        public ICommand InvitationDeniedCommand { get; set; }

        public MyPendingEmployeeInvitesPageViewModel() : base()
        {
            InvitationAcceptedCommand = new Command<EmployeeInvitationAndCompanyModel>(InvitationAccepted);
            InvitationDeniedCommand = new Command<EmployeeInvitationAndCompanyModel>(InvitationDenied);
        }

        protected override async void Load()
        {
            if(!Loading)
                Loading = true;

            if (MyEmployeeInvitations.Count != 0)
                MyEmployeeInvitations.Clear();
            
            var myPendingInvitations = await _db.GetMyPendingEmployeeInvites(_userData.Id);

            foreach (var invite in myPendingInvitations)
            {
                var company = await _db.GetCompany(invite.CompanyId);
                EmployeeInvitationAndCompanyModel model = new EmployeeInvitationAndCompanyModel()
                {
                    Company = company,
                    EmployeeInvitation = invite
                };

                MyEmployeeInvitations.Add(model);
            }

            Loading = false;
        }

        private async void InvitationAccepted(EmployeeInvitationAndCompanyModel invitation)
        {
            Loading = true;
            var company = await _db.GetCompany(invitation.EmployeeInvitation.CompanyId);
            var succeeded = await _db.AcceptEmployeeInvitation(_userData.Id, invitation.EmployeeInvitation.CompanyId);
            if (succeeded)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "You are now an employee of " + company.CompanyName, "Ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered trying to join this company", "Return");
            }
            Load();
        }

        private async void InvitationDenied(EmployeeInvitationAndCompanyModel invitation)
        {
            Loading = true;
            var company = await _db.GetCompany(invitation.EmployeeInvitation.CompanyId);
            var succeeded = await _db.AcceptEmployeeInvitation(_userData.Id, invitation.EmployeeInvitation.CompanyId);
            if (succeeded)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "You have denied the invitation from " + company.CompanyName, "Ok");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered trying to deny this invite", "Return");
            }
        }
    }
}