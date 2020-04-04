using Ecco.Api;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private IDatabaseManager _database;

        public ICommand LoginCommand { get; set; }
        public ICommand RegisterCommand { get; set; }

        public string EmailText { get; set; }
        public string PasswordText { get; set; }
        public string ConfirmPasswordText { get; set; }

        private bool _loading;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                OnPropertyChanged(nameof(Loading));
            }
        }

        public LoginViewModel()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            LoginCommand = new Command(async()  => await Login());
            RegisterCommand = new Command(async() => await Register());
        }

        public async Task<Task> Login()
        {
            Loading = true;
            var loginSuccesful = await _database.Login(EmailText, PasswordText);
            if (loginSuccesful)
            {
                Console.WriteLine("Succesfully logged in!");
                var userInfo = await _database.GetUserData();
                string userDataJson = JsonConvert.SerializeObject(userInfo);
                CrossSettings.Current.AddOrUpdateValue("UserData", userDataJson);
                SavePreferences(EmailText, PasswordText);
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

        public async Task<Task> Register()
        {
            var registerResult = await _database.Register(EmailText, PasswordText, ConfirmPasswordText);
            if (registerResult.StatusCode == 200)
            {
                Console.WriteLine("Registration Succesful!");
                await Application.Current.MainPage.DisplayAlert("Registration Complete", "You have succesfully registered your account!", "Return");
                await Login();
            }
            else 
            {
                Console.WriteLine("Registration Unsuccesful!");
                await Application.Current.MainPage.DisplayAlert("Registration Error", "Could not register with the given credentials", "Return");
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