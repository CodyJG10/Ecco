using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Ecco.Mobile.Util
{
    public static class OnboardingUtil
    {
        public static void ShowOnboardingIfNotSeenBefore(ContentPage page)
        {
            string keyName = "Onboarding_" + page.GetType().Name;
            if (!OnboardingHasBeenShown(keyName))
            {
                Application.Current.MainPage.Navigation.PushModalAsync(page);
                SetOnboardingHasBeenShown(keyName);
            }
        }

        private static bool OnboardingHasBeenShown(string name)
        {
            return CrossSettings.Current.Contains(name);
        }

        private static void SetOnboardingHasBeenShown(string name)
        {
            CrossSettings.Current.AddOrUpdateValue(name, true);
        }
    }
}