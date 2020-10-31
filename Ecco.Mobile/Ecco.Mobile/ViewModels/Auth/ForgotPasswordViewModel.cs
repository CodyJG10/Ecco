using Ecco.Api;
using Ecco.Mobile.Views.Authentication;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Auth
{
    public class ForgotPasswordViewModel : LoginViewModelBase
    {
        private IDatabaseManager _database;
        
        public ICommand ForgotPasswordCommand { get; set; }

        public ForgotPasswordViewModel() : base()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            ForgotPasswordCommand = new Command(ForgotPassword);
            LoginCommand = new Command(() => Application.Current.MainPage = new LoginPage());
        }

        private void ForgotPassword()
        {
            if (Email.Length == 0 || !Email.Contains("@"))
            {
                Application.Current.MainPage.DisplayAlert("Error", "Please enter a valid email", "Return");
                return;
            }
            _database.ForgotPassword(Email);
            Application.Current.MainPage.DisplayAlert("Reset Password Request Recieved", "Please check the email sent to you for a link to reset you password", "Ok");
            Application.Current.MainPage = new LoginPage();
        }
    }
}