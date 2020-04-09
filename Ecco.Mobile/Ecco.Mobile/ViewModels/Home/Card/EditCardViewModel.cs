using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Nancy.TinyIoc;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class EditCardViewModel : ViewModelBase
    {
        private IDatabaseManager _db;

        public CardModel Card { get; set; }
        
        public ICommand SaveCommand { get; set; }

        public EditCardViewModel(CardModel card)
        {
            Card = card;

            _db = TinyIoCContainer.Current.Resolve(typeof(IDatabaseManager)) as IDatabaseManager;

            SaveCommand = new Command(Save);
        }

        private async void Save()
        {
            var succesful = await _db.EditCard(Card.Card);
            if (succesful)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Success", "Changes saved", "Ok");
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered while trying to save your changes. Please try again later.", "Ok");
            }
        }
    }
}