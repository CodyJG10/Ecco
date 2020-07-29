using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Views.Authentication;
using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages.CompanyPages;
using Nancy.TinyIoc;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.HomeMaster
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeMasterMaster : ContentPage
    {
        public ListView ListView;

        public HomeMasterMaster()
        {
            InitializeComponent();
            BindingContext = new HomeMasterMasterViewModel();
            ListView = MenuItemsListView;
        }

        class HomeMasterMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<HomeMasterMasterMenuItem> MenuItems { get; set; }

            public HomeMasterMasterViewModel()
            {
                MenuItems = new ObservableCollection<HomeMasterMasterMenuItem>(new[]
                {
                    new HomeMasterMasterMenuItem { Id = 0, Title = "Configure ECCO Card", OnClicked = () => 
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new WriteTag());
                    }},
                    new HomeMasterMasterMenuItem { Id = 1, Title = "Scan ECCO Card", OnClicked = () => 
                    {
                        Application.Current.MainPage.Navigation.PushAsync(new ReadTagPage());
                    }},
                    new HomeMasterMasterMenuItem { Id = 3, Title = "Log Out", OnClicked = () => 
                    {
                        CrossSettings.Current.Remove("UserData");
                        CrossSettings.Current.Remove("Username");
                        CrossSettings.Current.Remove("Password");
                        CrossSettings.Current.Remove("RefreshToken");
                        (TinyIoCContainer.Current.Resolve<AutoUpdater>()).Stop();
                        TinyIoCContainer.Current.Unregister(typeof(AutoUpdater));
                        Application.Current.MainPage = new LoginPage();
                    }},
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}