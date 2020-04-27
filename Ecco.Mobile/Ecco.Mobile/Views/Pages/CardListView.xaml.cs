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
    public partial class CardListView : ContentPage
    {
        private ConnectionModel _swipedCard;

        public CardListView()
        {
            InitializeComponent();
        }

        public void Refresh()
        { 
            (BindingContext as CardListViewModel).RefreshCommand.Execute(null);
        }

        private void CardholderNameSearchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var searchBar = (sender as SearchBar);
            if (ConnectionsList.DataSource != null)
            {
                ConnectionsList.DataSource.Filter = FilterCards;
                ConnectionsList.DataSource.RefreshFilter();
            }
        }

        private bool FilterCards(object obj)
        {
            if (CardholderNameSearchbar.Text == null)
                return true;

            var connection = obj as ConnectionModel;
            if (connection.Card.Card.FullName.ToLower().Contains(CardholderNameSearchbar.Text.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void ConnectionsList_SwipeStarted(object sender, Syncfusion.ListView.XForms.SwipeStartedEventArgs e)
        {
            _swipedCard = e.ItemData as ConnectionModel;
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
                    (BindingContext as CardListViewModel).DeleteConnectionCommand.Execute(_swipedCard);
                    popupLayout.IsOpen = false;
                    (BindingContext as CardListViewModel).RefreshCommand.Execute(null);
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