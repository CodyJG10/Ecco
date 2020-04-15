using Ecco.Api;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.Models
{
    public class CardModel
    {
        public Entities.Card Card { get; set; }
        public ImageSource TemplateImage { get; set; }

        public static async Task<CardModel> FromCard(Entities.Card card)
        {
            IDatabaseManager db = TinyIoCContainer.Current.Resolve<IDatabaseManager>() as IDatabaseManager;
            IStorageManager storage = TinyIoCContainer.Current.Resolve<IStorageManager>() as IStorageManager;
            return new CardModel()
            {
                Card = card,
                TemplateImage = await TemplateUtil.LoadImageSource(card, db, storage)
            };
        }
    }
}