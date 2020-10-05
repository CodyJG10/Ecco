using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyCardView : ContentView
    {
        //private CardModel _swipedCard;

        public MyCardView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                InformationStack.Spacing = 25;
            }
        }

        private void SfCardLayout_VisibleCardIndexChanged(object sender, Syncfusion.XForms.Cards.VisibleCardIndexChangedEventArgs e)
        {
            CardModel newCard = e.NewCard.BindingContext as CardModel;
            ((MyCardViewModel)BindingContext).ShowCardInfo(newCard);
        }

        //private void CardLayout_ItemAppeared(PanCardView.CardsView view, PanCardView.EventArgs.ItemAppearedEventArgs args)
        //{
        //    CardModel newCard = args.Item as CardModel;
        //    ((MyCardViewModel)BindingContext).ShowCardInfo(newCard);
        //}
    }
}