using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Onboarding
{
    public class ConnectionsModalViewModel : OnboardingViewModelBase
    {
        public ConnectionsModalViewModel() : base() { }

        protected override void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingOverviewModel>
            {
                new OnboardingOverviewModel
                {
                    Title = "The Connections Page",
                    Content = "The Connections page allows you to easily manage, interact, & navigate the Ecco business cards you've recieved",
                    ImageUrl = "ecco_logo.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Easily Organize & Find Other People's Business Cards",
                    Content = "Looking for a spefic person or maybe a certain service? \n" +
                              "With the filter dropdown, you can easily filter and view cards in your network to your specifications.",
                    ImageUrl = "onboarding_create_card.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Get In Contact With Someone",
                    Content = "With just a simple tap of anybody's card, you can easily reach get in contact" +
                              "in the form of a phone call, email, and even add the person to your phones contact list with a single tap!",
                    ImageUrl = "onboarding_card"
                }
            };
        }
    }
}