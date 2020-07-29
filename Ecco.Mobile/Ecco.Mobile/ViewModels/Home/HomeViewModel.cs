using Ecco.Mobile.Views.Pages;
using Ecco.Mobile.Views.Pages.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        private int _selectedIndex = 1;
        public int SelectedIndex
        {
            get
            {
                return _selectedIndex;
            }
            set
            {
                _selectedIndex = value;
                OnPropertyChanged(nameof(SelectedIndex));
            }
        }
    }
}
