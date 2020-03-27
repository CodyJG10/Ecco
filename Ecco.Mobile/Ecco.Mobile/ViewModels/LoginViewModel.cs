using Ecco.Api;
using Nancy.TinyIoc;
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

        public string EmailText { get; set; } = "codyjg10@gmail.com";
        public string PasswordText { get; set; } = "Airplane10";
        public string ConfirmPasswordText { get; set; } = "";

        public LoginViewModel()
        {
            _database = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            LoginCommand = new Command(async()  => await Login());
            RegisterCommand = new Command(async() => await Register());
        }

        public async Task<Task> Login()
        {
            var loginSuccesful = await _database.Login(EmailText, PasswordText);
            if (loginSuccesful)
            {
                Console.WriteLine("Succesfully logged in!");
            }
            else
            {
                Console.WriteLine("Login was unsuccesful");
            }
            return Task.CompletedTask;
        }

        public async Task<Task> Register()
        {
            var registerResult = await _database.Register(EmailText, PasswordText, ConfirmPasswordText);
            if (registerResult.StatusCode == 200)
            {
                Console.WriteLine("Registration Succesful!");
            }
            else 
            {
                Console.WriteLine("Registration Unsuccesful!");
            }
            return Task.CompletedTask;
        }
    }
}