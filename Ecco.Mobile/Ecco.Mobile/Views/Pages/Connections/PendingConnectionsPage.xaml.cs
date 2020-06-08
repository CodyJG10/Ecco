using Ecco.Mobile.ViewModels.Home.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages.Connections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PendingConnectionsPage : ContentPage
    {
        public PendingConnectionsPage(Action refreshAction)
        {
            InitializeComponent();
            BindingContext = new PendingConnectionsViewModel(refreshAction);
        }
    }
}