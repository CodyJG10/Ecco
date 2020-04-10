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
    public partial class MyCardView : ContentPage
    {
        private CardModel _swipedCard;

        public MyCardView()
        {
            InitializeComponent();
        }

        private void ButtonDeleteCard_Clicked(object sender, EventArgs e)
        {
            var popupLayout = new SfPopupLayout();


            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Label();
                popupContent.Text = "Are you sure you want to delete this card?";
                popupContent.BackgroundColor = Color.LightSkyBlue;
                popupContent.HorizontalTextAlignment = TextAlignment.Center;
                return popupContent;
            });

            var footerTemplateView = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center
                };

                var yesButton = new Button()
                {
                    Text = "Yes",
                };
                var noButton = new Button()
                {
                    Text = "No"
                };
                noButton.Clicked += (s, a) =>
                {
                    popupLayout.IsOpen = false;
                };
                yesButton.Clicked += (s, a) =>
                {
                    (BindingContext as MyCardViewModel).DeleteCardCommand.Execute(_swipedCard);
                    popupLayout.IsOpen = false;
                    (BindingContext as MyCardViewModel).RefreshCommand.Execute(null); 
                };

                stackLayout.Children.Add(yesButton);
                stackLayout.Children.Add(noButton); 

                return stackLayout;
            });

            popupLayout.PopupView.ContentTemplate = templateView;
            popupLayout.PopupView.FooterTemplate = footerTemplateView;

            popupLayout.Show();
        }

        private void CardList_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            _swipedCard = e.ItemData as CardModel;
        }

        private void ButtonEditCard_Clicked(object sender, EventArgs e)
        {
            (BindingContext as MyCardViewModel).EditCardCommand.Execute(_swipedCard);
        }
    }
}