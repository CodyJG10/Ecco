using Ecco.Mobile.Views.Onboarding;
using Ecco.Mobile.Views.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : TabbedPage
    {
        private bool _isFirstPage = true;

        public Home()
        {
            InitializeComponent();
            HomePage.CurrentPage = HomePage.Children[2];
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (_isFirstPage)
            {
                _isFirstPage = false;
                return;
            }

            if (HomePage.CurrentPage.GetType() == typeof(CardListView))
            {
                Application.Current.MainPage.Navigation.PushModalAsync(new ConnectionsModal());
            }

            else if (HomePage.CurrentPage.GetType() == typeof(MyCardView))
            { 
                Application.Current.MainPage.Navigation.PushModalAsync(new MyCardsModal());
            }
        }
    }
}