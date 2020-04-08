using Ecco.Api;
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
    public class LoginPageViewModel : LoginViewModelBase
    { 
        private IDatabaseManager _database;

        public LoginPageViewModel()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            LoginCommand = new Command(async () => await Login());
            RegisterCommand = new Command(Register);
        }

        public async Task<Task> Login()
        {
            Loading = true;
            var loginSuccesful = await _database.Login(Email, Password);
            if (loginSuccesful)
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
                Console.WriteLine("Login was unsuccesful");
                await Application.Current.MainPage.DisplayAlert("Authentication Error", "The provided credentials were incorrect", "Return");
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