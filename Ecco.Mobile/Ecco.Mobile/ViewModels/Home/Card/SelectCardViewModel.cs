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
        public Entities.Card Card { get; set; }

        public ICommand CallCommand { get; set; }
        public ICommand EmailCommand { get; set; }

        public SelectCardViewModel(Entities.Card card)
        {
            Card = card;

            CallCommand = new Command(Call);
            EmailCommand = new Command(Email);
        }

        private void Call()
        {
            Launcher.OpenAsync(new Uri("tel:" + Card.Phone));
        }

        private void Email()
        { 
            Launcher.OpenAsync(new Uri("mailto:" + Card.Email));
        }
    }
}
