using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.ViewModels.Home
{
    public class HomeMasterViewModel : LoadingViewModel
    {
        private string _profileName = "Profile Name";
        public string ProfileName
        {
            get
            {
                return _profileName;
            }
            set
            {
                _profileName = value;
                OnPropertyChanged(nameof(ProfileName));
            }
        }

        public HomeMasterViewModel() : base() { }

        protected override void Load()
        { 
            ProfileName = _userData.ProfileName;
        }
    }
}