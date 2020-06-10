using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Ecco.Mobile.ViewModels.Home.Card;
using Syncfusion.SfBarcode.XForms;
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
    public partial class MyCard : ContentPage
    {
        public MyCard(CardModel model)
        {
            InitializeComponent();
            BindingContext = new ViewMyCardViewModel(model);
        }
    }
}