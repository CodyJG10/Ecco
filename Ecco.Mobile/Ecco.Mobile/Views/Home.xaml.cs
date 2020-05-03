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
        private bool _isFirstLaunch = true;

        public Home()
        {
            InitializeComponent();
        }

        private void TabbedPage_CurrentPageChanged(object sender, EventArgs e)
        {
            if (_isFirstLaunch)
            {
                _isFirstLaunch = false;
                return;
            }

            if (HomePage.CurrentPage.GetType() == typeof(CardListView))
            {
                OnboardingUtil.ShowOnboardingIfNotSeenBefore(new ConnectionsModal());
            }

            else if (HomePage.CurrentPage.GetType() == typeof(MyCardView))
            {
                OnboardingUtil.ShowOnboardingIfNotSeenBefore(new MyCardsModal());
            }
        }

        private bool _shownWelcomeModal = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (_shownWelcomeModal == false)
            { 
                OnboardingUtil.ShowOnboardingIfNotSeenBefore(new WelcomeModal());
                _shownWelcomeModal = true;
            }
        }
    }
}