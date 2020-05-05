using Ecco.Api;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Authentication;
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
    public class RegisterPageViewModel : LoginViewModelBase
    {
        private IDatabaseManager _database;

        public string Username { get; set; }
        public string ConfirmPasswordText { get; set; }

        public RegisterPageViewModel()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            LoginCommand = new Command(Login);
            RegisterCommand = new Command(async () => await Register());        
        }

        public void Login()
        {
            Application.Current.MainPage = new LoginPage();
        }

        public async Task<Task> Register()
        {
            if (Loading) return Task.CompletedTask;
            Loading = true;
            var registrationResponse = await _database.Register(Username, Email, Password, ConfirmPasswordText);
            if (registrationResponse.IsSuccessStatusCode)
            {
                //Registration Complete
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
            }
            else
            {
                var error = await HttpResponseFormatter.GetErrorFromResponse(registrationResponse);
                await Application.Current.MainPage.DisplayAlert("Registration Error", error, "Return");
                Loading = false;
            }
            return Task.CompletedTask;
        }

        private void SavePreferences(string username, string password)
        {
            CrossSettings.Current.AddOrUpdateValue("Username", username);
            CrossSettings.Current.AddOrUpdateValue("Password", password);
        }
    }
}
