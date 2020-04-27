using Ecco.Api;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Authentication;
using Nancy;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Auth
{
    public class LoginPageViewModel : LoginViewModelBase
    { 
        private readonly IDatabaseManager _database;
        
        public ICommand ForgotPasswordCommand { get; set; }

        public LoginPageViewModel()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(Register);
            ForgotPasswordCommand = new Command(ForgotPassword);
        }

        private void ForgotPassword()
        {
            Application.Current.MainPage.DisplayAlert("Reset Password Request Recieved", "Please check the email sent to you for a link to reset you password", "Ok");
        }

        public async Task<Task> Login()
        {
            Loading = true;
            var loginResponse = await _database.Login(Email, Password);
            if (loginResponse.IsSuccessStatusCode)
            {
                Console.WriteLine("Succesfully logged in!");
                var userInfo = await _database.GetUserData();
                string userDataJson = JsonConvert.SerializeObject(userInfo);
                CrossSettings.Current.AddOrUpdateValue("UserData", userDataJson);
                SavePreferences(Email, Password);
                Application.Current.MainPage = new NavigationPage(new Views.Home());

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Authentication Error", "Please make sure you've entered the correct credentials", "Return");
            }
            Loading = false;
            return Task.CompletedTask;
        }

        public void Register()
        {
            Application.Current.MainPage = new RegisterPage();
        }

        private void SavePreferences(string username, string password)
        {
            CrossSettings.Current.AddOrUpdateValue("Username", username);
            CrossSettings.Current.AddOrUpdateValue("Password", password);
        }
    }
}