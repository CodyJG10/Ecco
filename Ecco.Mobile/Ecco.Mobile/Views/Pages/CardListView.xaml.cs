﻿using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
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
        private CardModel _swipedCard;

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
            _swipedCard = e.ItemData as CardModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var popupLayout = new SfPopupLayout();

            var templateView = new DataTemplate(() =>
            {
                var popupContent = new Label();
                popupContent.Text = "Are you sure you want to delete this users card from your connections list?";
                popupContent.BackgroundColor = Color.White;
                popupContent.HorizontalTextAlignment = TextAlignment.Center;
                return popupContent;
            });

            var footerTemplateView = new DataTemplate(() =>
            {
                var stackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
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