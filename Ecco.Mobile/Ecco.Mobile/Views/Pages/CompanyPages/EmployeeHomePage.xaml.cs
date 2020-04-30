using Ecco.Entities.Company;
using Ecco.Mobile.ViewModels.CompanyPages;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
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

        private void ButtonLeave_Clicked(object sender, EventArgs e)
        {
            var viewModel = OptionsMenu.BindingContext as EmployeeHomePageViewModel;
            viewModel.LeaveCompanyCommand.Execute(_selectedCompany);
        }

        private void ListMyEmployers_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            _selectedCompany = ListMyEmployers.SelectedItem as Company;
            OptionsMenu.ShowAtTouchPoint();
        }
    }
}