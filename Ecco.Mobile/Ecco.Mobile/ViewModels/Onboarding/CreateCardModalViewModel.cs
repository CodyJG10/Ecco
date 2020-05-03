using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Ecco.Mobile.ViewModels.Onboarding
{
    public class CreateCardModalViewModel : OnboardingViewModelBase
    {
        public CreateCardModalViewModel() : base() { }

        protected override void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingOverviewModel>
            {
                new OnboardingOverviewModel
                {
                    Title = "Creating A Business Card",
                    Content = "Creating a card is extremely simple. Just follow these 3 steps and unlike with paper business cards, you can spend more time networking than waiting!",
                    ImageUrl = "onboarding_card.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Step 1",
                    Content = "Provide some basic information. \n" +
                              "This makes it as easy as possible for people to get in contact with you when they recieve your card!",
                    ImageUrl = "onboarding_info.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Step 2",
                    Content = "Next, explore the list of templates that you can select to be the background of your new business card.",
                    ImageUrl = "onboarding_templates.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Step 3",
                    Content = "Finally, use the drag & drop editor to add / edit text, add shapes, and adjust image settings to bring your business card to life.",
                    ImageUrl = "onboarding_editor.png"
                }
            };
        }
    }
}