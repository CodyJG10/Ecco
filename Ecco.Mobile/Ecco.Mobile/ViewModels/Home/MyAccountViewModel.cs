using Ecco.Api;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class MyAccountViewModel : ViewModelBase
    {
        private IDatabaseManager _db;
        private UserData _userData;

        public MyAccountViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
        }
    }
}
