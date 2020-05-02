using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Onboarding
{
    public class OnboardingWelcomeScreenViewModel : ViewModelBase
    {
        private int position;
        public int Position
        {
            get => position;
            set
            {
                position = value;
                UpdateSkipButtonText();
            }
        }

        public ObservableCollection<OnboardingOverviewModel> Items { get; set; }

        private string skipButtonText;
        public string SkipButtonText
        {
            get => skipButtonText;
            set 
            {
                skipButtonText = value;
                OnPropertyChanged(nameof(SkipButtonText));
            }
        }

        public ICommand SkipCommand { get; private set; }

        public OnboardingWelcomeScreenViewModel()
        {
            SkipButtonText = "SKIP";
            InitializeOnBoarding();
            InitializeSkipCommand();
        }

        private void InitializeOnBoarding()
        {
            Items = new ObservableCollection<OnboardingOverviewModel>
            {
                new OnboardingOverviewModel
                {
                    Title = "Welcome to \n TABIA",
                    Content = "Tabia helps you build habits that stick.",
                    ImageUrl = "icon.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Reminder",
                    Content = "Reminder helps you execute your habits each day.",
                    ImageUrl = "onboarding_create_card.png"
                },
                new OnboardingOverviewModel
                {
                    Title = "Track your progress",
                    Content = "Charts help you visualize your efforts over time.",
                    ImageUrl = "onboarding_card"
                }
            };
        }

        private void InitializeSkipCommand()
        {
            SkipCommand = new Command(() =>
            {
                if (LastPositionReached())
                {
                    ExitOnBoarding();
                }
                else
                {
                    MoveToNextPosition();
                }
            });
        }

        private static void ExitOnBoarding()
            => Application.Current.MainPage.Navigation.PopModalAsync();

        private void MoveToNextPosition()
        {
            var nextPosition = ++Position;
            Position = nextPosition;
        }

        private bool LastPositionReached()
            => Position == Items.Count - 1;      

        private void UpdateSkipButtonText()
        {
            if (LastPositionReached())
            {
                SkipButtonText = "GOT IT";
            }
            else
            {
                SkipButtonText = "SKIP";
            }
        }
    }
}
