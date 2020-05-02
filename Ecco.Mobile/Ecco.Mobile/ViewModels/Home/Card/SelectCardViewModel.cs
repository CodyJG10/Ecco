using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views.NFC;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class SelectCardViewModel
    {
        public CardModel Card { get; set; }

        public ICommand CallCommand { get; set; }
        public ICommand EmailCommand { get; set; }
        public ICommand AddToContactsCommand { get; set; }

        public SelectCardViewModel(CardModel card)
        {
            Card = card;

            CallCommand = new Command(Call);
            EmailCommand = new Command(Email);
            AddToContactsCommand = new Command(AddToContacts);
        }

        private void Call()
        {
            Launcher.OpenAsync(new Uri("tel:" + Card.Card.Phone));
        }

        private void Email()
        { 
            Launcher.OpenAsync(new Uri("mailto:" + Card.Card.Email));
        }

        private async void AddToContacts()
        {
            var permissionStatus = await Permissions.CheckStatusAsync<Permissions.ContactsWrite>();
            if (permissionStatus != PermissionStatus.Granted)
            { 
                var status = await Permissions.RequestAsync<Permissions.ContactsWrite>();
                if (status != PermissionStatus.Granted)
                {
                    return;
                }
            }
            DependencyService.Get<ISaveContact>().SaveContact(Card.Card.FullName, Card.Card.Phone, Card.Card.Email);
        }
    }
}
