using Ecco.Api;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecco.Mobile.Models
{
    public class CardModel
    {
        public Entities.Card Card { get; set; }
        public ImageSource TemplateImage { get; set; }
    }
}