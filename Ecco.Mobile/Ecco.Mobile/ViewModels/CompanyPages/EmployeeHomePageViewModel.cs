using Ecco.Api;
using Ecco.Entities.Company;
using Ecco.Mobile.Views.Pages.CompanyPages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.CompanyPages
{
    public class EmployeeHomePageViewModel : LoadingViewModel
    {
        private bool _hasPendingInvitations;
        public bool HasPendingInvitations
        {
            get
            {
                return _hasPendingInvitations;
            }
            set
            {
                _hasPendingInvitations = value;
                OnPropertyChanged(nameof(HasPendingInvitations));
            }
        }

        public ObservableCollection<Company> MyEmployers { get; set; } = new ObservableCollection<Company>();

        public ICommand ViewPendingInvitationsCommand { get; set; }
        public ICommand LeaveCompanyCommand { get; set; }

        public EmployeeHomePageViewModel() : base()
        {
            ViewPendingInvitationsCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new MyPendingEmployeeInvitesPage()));
            LeaveCompanyCommand = new Command<Company>(LeaveCompany);
        }

        private void LeaveCompany(Company company)
        { 
            //TODO
        }

        protected async override void Load()
        {
            Loading = true;

            if (MyEmployers.Count != 0)
                MyEmployers.Clear();

            var myEmployers = await _db.GetMyEmployers(_userData.Id.ToString());
            myEmployers.ForEach(x => MyEmployers.Add(x));

            var pendingInvitations = await _db.GetMyPendingEmployeeInvites(_userData.Id);
            HasPendingInvitations = pendingInvitations.Count != 0;

            Loading = false;
        }
    }
}
