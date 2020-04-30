using Ecco.Entities.Company;
using Ecco.Mobile.ViewModels.CompanyPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages.CompanyPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeHomePage : ContentPage
    {
        private Company _selectedCompany;

        public EmployeeHomePage()
        {
            InitializeComponent();
        }

        private void ButtonLeaveCompany_Clicked(object sender, EventArgs e)
        {
            (BindingContext as EmployeeHomePageViewModel).LeaveCompanyCommand.Execute(_selectedCompany);
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            _selectedCompany = ListMyEmployers.SelectedItem as Company;
            OptionsPopup.ShowAtTouchPoint();
        }
    }
}