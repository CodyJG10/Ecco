using Ecco.Mobile.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SendCard : ContentPage
    {
        private SendCardViewModel ViewModel
        {
            get
            {
                return BindingContext as SendCardViewModel;
            }
        }

        public SendCard()
        {
            InitializeComponent();
        }

        private void entryUserQuery_TextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.UserSearchTypedCommand.Execute(null);
        }

        private void listMyCards_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            ViewModel.CardSelectedCommand.Execute(listMyCards.SelectedItem);
        }

        private void ListUserResults_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            ViewModel.UserSelectedCommand.Execute(ListUserResults.SelectedItem);
        }
    }
}