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

        public ScanCardViewModel()
        {
            SendConnectionCommand = new Command(x => Application.Current.MainPage.Navigation.PushAsync(new SendCard()));
        }
    }
}
