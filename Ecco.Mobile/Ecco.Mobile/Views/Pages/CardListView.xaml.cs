using Ecco.Mobile.Models;
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
    }
}