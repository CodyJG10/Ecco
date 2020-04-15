﻿using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages;
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
        public ICommand TestNfcCommand { get; set; }
        public ICommand WriteTagCommand { get; set; }

        public ScanCardViewModel()
        {
            SendConnectionCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new SendCard()));
            TestNfcCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new NfcTestPage()));
            WriteTagCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new WriteTag()));
        }
    }
}
