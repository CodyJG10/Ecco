using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewCardPage : ContentPage
    {
        public ViewCardPage(CardModel card)
        {
            InitializeComponent();
            BindingContext = new SelectCardViewModel(card);
        }
    }
}