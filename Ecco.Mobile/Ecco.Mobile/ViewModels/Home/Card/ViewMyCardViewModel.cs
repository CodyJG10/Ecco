using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class ViewMyCardViewModel : ViewModelBase
    {
        public CardModel CardModel { get; set; }
        public string QrCodeUrl { get; set; }

        public ICommand ShareCommand { get; set; }

        public ViewMyCardViewModel(CardModel model)
        {
            CardModel = model;
            QrCodeUrl = "https://ecco-space.azurewebsites.net/cards/" + model.Card.Id.ToString();
            ShareCommand = new Command(Share);
        }

        public async void Share()
        {
            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest()
            {
                Uri = QrCodeUrl,
                Text = "Hey, check out my digital business card on Ecco Space!",
                Title = "Digital Business Card"
            });
        }
    }
}