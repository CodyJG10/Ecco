using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Onboarding;
using Ecco.Mobile.Views.Pages;
using Plugin.Settings;
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
        public Home()
        {
            InitializeComponent();
            HomePage.CurrentPage = HomePage.Children[2];
            OnboardingUtil.ShowOnboardingIfNotSeenBefore(new WelcomeModal());
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (HomePage.CurrentPage.GetType() == typeof(CardListView))
            {
                OnboardingUtil.ShowOnboardingIfNotSeenBefore(new ConnectionsModal());
            }

            else if (HomePage.CurrentPage.GetType() == typeof(MyCardView))
            {
                OnboardingUtil.ShowOnboardingIfNotSeenBefore(new MyCardsModal());
            }
        }
    }
}