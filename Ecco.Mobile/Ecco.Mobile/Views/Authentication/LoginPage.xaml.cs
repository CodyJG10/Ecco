using Ecco.Mobile.ViewModels.Auth;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            string email = "";
            var popupLayout = new SfPopupLayout();
            popupLayout.PopupView.AnimationMode = AnimationMode.Zoom;
            popupLayout.PopupView.ShowHeader = false;

            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Entry()
                {
                    Placeholder = "Email",
                    HorizontalOptions = LayoutOptions.Center
                };

                popupContent.TextChanged += (obj, args) =>
                {
                    email = popupContent.Text;
                };

                return popupContent;
            });

            var footerTemplateView = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                };

                var yesButton = new SfButton()
                {
                    Text = "Submit",
                    WidthRequest = 65d,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };

                yesButton.Clicked += (s, a) =>
                {
                    (BindingContext as LoginPageViewModel).ForgotPasswordCommand.Execute(email);
                    popupLayout.IsOpen = false;
                };

                stackLayout.Children.Add(yesButton);

                return stackLayout;
            });

            popupLayout.PopupView.ContentTemplate = templateView;
            popupLayout.PopupView.FooterTemplate = footerTemplateView;

            popupLayout.Show();
        }
    }
}