using Ecco.Mobile.Models;
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
    public partial class CreateCardPage : ContentPage
    {
        public CreateCardPage()
        {
            InitializeComponent();
        }

        private void SfListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            var model = e.ItemData as TemplateModel;
            (BindingContext as CreateCardViewModel).TemplateSelectedCommand.Execute(model);
        }
    }
}