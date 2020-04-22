using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class ViewMyCardViewModel : ViewModelBase
    {
        public CardModel CardModel { get; set; }
        public string QrCodeUrl { get; set; }

        public ViewMyCardViewModel(CardModel model)
        {
            CardModel = model;
            QrCodeUrl = "https://ecco-space.azurewebsites.net/cards/" + model.Card.Id.ToString();
        }
    }
}