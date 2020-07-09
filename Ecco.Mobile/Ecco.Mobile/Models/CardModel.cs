using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.Models
{
    public class CardModel
    {
        public Card Card { get; set; }
        public ImageSource CardImage { get; set; }
        public bool IsActiveCard { get; set;  }

        public static CardModel FromCard(Card card, UserData user)
        {
            return new CardModel()
            {
                Card = card,
                CardImage = ImageLoader.LoadCardImage(card, user.UserName)
            };
        }
    }
}