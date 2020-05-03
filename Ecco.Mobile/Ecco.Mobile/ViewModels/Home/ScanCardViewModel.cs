using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Onboarding;
using Ecco.Mobile.Views.Pages;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home
{
    public class ScanCardViewModel : ViewModelBase
    {
        public ICommand SendConnectionCommand { get; set; }
        public ICommand EccoCardCommand { get; set; }

        public ScanCardViewModel()
        {
            SendConnectionCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new SendCard()));
            EccoCardCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new EccoCardPage()));
        }
    }
}