using Ecco.Api;
using Ecco.Entities;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class CreateCardViewModel : ViewModelBase
    {
        public string CardTitle { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICommand CreateCommand { get; set; }

        private IDatabaseManager _db;

        public CreateCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            CreateCommand = new Command(CreateCard);
        }

        public async void CreateCard()
        {
            UserData user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            Entities.Card card = new Entities.Card()
            {
                CardTitle = CardTitle,
                Description = Description,
                Email = Email,
                JobTitle = JobTitle,
                Phone = Phone,
                UserId = user.Id
            };
            var succeeded = await _db.CreateCard(card);
            if (succeeded)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else 
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card. Please ensure all information is filled in", "Ok");
            }
        }
    }
}