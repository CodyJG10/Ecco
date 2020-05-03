using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Ecco.Mobile.ViewModels.Onboarding
{
    public class MyCardsModalViewModel : OnboardingViewModelBase
    {
        public MyCardsModalViewModel() : base() { }

        protected override void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingOverviewModel>
            {
                new OnboardingOverviewModel
                {
                    Title = "The My Cards Page",
                    Content = "The My Cards page allows you to create, edit, manage, & share your Ecco business cards.",
                    ImageUrl = "onboarding_card.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Manage Your Cards",
                    Content = "A quick swipe on any of your cards will reveal options to delete or edit your card.",
                    ImageUrl = "onboarding_management.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Share Your Card",
                    Content = "Easily share your cards with other people by tapping your card of choice. \n" + 
                              "To allow for the most versabliity, you can share your cards via any method such as link, email, text, and of course, a physical Ecco Card!",
                    ImageUrl = "onboarding_share.png"
                }
            };
        }
    }
}
