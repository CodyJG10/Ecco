using Ecco.Mobile.ViewModels.CompanyPages;
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
    public partial class MyCompanyPage : ContentPage
    {
        public MyCompanyPage()
        {
            InitializeComponent();
        }

        private void DeleteCompanyButton_Clicked(object sender, EventArgs e)
        {
            var popupLayout = new SfPopupLayout();

            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Label
                {
                    Text = "Are you sure you want to delete your company? All of your employee data and company information will be erased permanently",
                    HorizontalTextAlignment = TextAlignment.Center,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.CenterAndExpand
                };
                return popupContent;
            });

            popupLayout.PopupView.ShowHeader = false;
            popupLayout.PopupView.AppearanceMode = AppearanceMode.TwoButton;

            popupLayout.PopupView.AcceptCommand = (BindingContext as MyCompanyPageViewModel).DeleteCompanyCommand;

            popupLayout.PopupView.ContentTemplate = templateView;

            popupLayout.Show();
        }
    }
}