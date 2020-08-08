using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Ecco.Mobile.ViewModels.Home.Card;
using Syncfusion.SfBarcode.XForms;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.PopupLayout;
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

        private void ButtonDeleteCard_Clicked(object sender, EventArgs e)
        {
            var popupLayout = new SfPopupLayout();
            popupLayout.PopupView.AnimationMode = AnimationMode.Zoom;
            popupLayout.PopupView.ShowHeader = false;

            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Label
                {
                    Text = "Are you sure you want to delete this card?",
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18,
                };

                return popupContent;
            });

            var footerTemplateView = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                };

                var yesButton = new SfButton()
                {
                    Text = "Yes",
                    WidthRequest = 65d,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };
                var noButton = new SfButton()
                {
                    Text = "No",
                    WidthRequest = 65d,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                };

                noButton.Clicked += (s, a) =>
                {
                    popupLayout.IsOpen = false;
                };
                yesButton.Clicked += (s, a) =>
                {
                    (BindingContext as ViewMyCardViewModel).DeleteCardCommand.Execute(null);
                    popupLayout.IsOpen = false;
                };

                stackLayout.Children.Add(yesButton);
                stackLayout.Children.Add(noButton);

                return stackLayout;
            });

            popupLayout.PopupView.ContentTemplate = templateView;
            popupLayout.PopupView.FooterTemplate = footerTemplateView;

            popupLayout.Show();
        }
    }
}