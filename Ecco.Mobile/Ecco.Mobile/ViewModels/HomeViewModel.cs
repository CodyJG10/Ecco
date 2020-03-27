using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private string _usernameText;
        public string UsernameText
        {
            get
            {
                return _usernameText;
            }
            set
            {
                _usernameText = value;
                OnPropertyChanged(nameof(UsernameText));
            }
        }

        private string _idText;
        public string IdText
        {
            get
            {
                return _idText;
            }
            set
            {
                _idText = value;
                OnPropertyChanged(nameof(IdText));
            }
        }

        public HomeViewModel()
        {
            string userDataJson = CrossSettings.Current.GetValueOrDefault("UserData", "");
            var userData = JsonConvert.DeserializeObject<UserData>(userDataJson);
            UsernameText = userData.UserName;
            IdText = userData.Id.ToString();
        }
    }
}
