using Ecco.Mobile.ViewModels.Auth;
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
    public partial class ForgotPasswordPage : ContentPage
    {
        public ForgotPasswordPage()
        {
            InitializeComponent();
        }

        //private void btnForgotPassword_Clicked(object sender, EventArgs e)
        //{
        //    string email = entryEmail.GetEmail();
        //    (BindingContext as LoginPageViewModel).ForgotPasswordCommand.Execute(email);
        //}
    }
}