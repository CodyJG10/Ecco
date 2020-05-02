using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Onboarding
{
    public class OnboardingWelcomeScreenViewModel : OnboardingViewModelBase
    {
        public OnboardingWelcomeScreenViewModel() : base() { } 

        protected override void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingOverviewModel>
            {
                new OnboardingOverviewModel
                {
                    Title = "Welcome to \n Ecco Space!",
                    Content = "Ecco Space removes the hassle of old fashioned & inconvenient business cards with a digital, advanced, & effective online approach.",
                    ImageUrl = "ecco_logo.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Create A Business Card",
                    Content = "Create your business card to your own speicification using a simple drag & drop editor",
                    ImageUrl = "onboarding_create_card.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Grow Your Network",
                    Content = "You can easily keep track of Ecco business cards you recieve \n \n" +
                               "The advantage of using Ecco business cards is the versability and technological capabilities \n \n" +
                               "Your paper business card can't dial a phone can it? Well, Ecco business cards can!",
                    ImageUrl = "onboarding_card"
                }
            };
        }
    }
}
