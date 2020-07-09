using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.Converters
{
    public class IsActiveCardColorConverter : IValueConverter
    {
        public  object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var card = value as CardModel;

            if (card == null)
                return Color.Red;

            if (card.IsActiveCard)
                return Color.Green;
            else
                return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return Color.White;
        }
    }
}