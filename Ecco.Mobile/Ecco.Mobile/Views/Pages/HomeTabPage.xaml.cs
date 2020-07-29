using Ecco.Mobile.Views.NFC;
using Sharpnado.Presentation.Forms.CustomViews.Tabs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeTabPage : ContentPage
    {
        public HomeTabPage()
        {
            InitializeComponent();
            Switcher.SelectedIndex = 0;
            typeof(ViewSwitcher).GetMethod("UpdateSelectedView", BindingFlags.NonPublic | BindingFlags.Instance)
                            .Invoke(Switcher, new object[] { 0 });
        }

        private void ButtonScanCard_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage.Navigation.PushAsync(new ReadTagPage());
        }
    }
}