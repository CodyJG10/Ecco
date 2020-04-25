using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCardEditor : ContentPage
    {
        public CreateCardEditor(CardModel model)
        {
            InitializeComponent();
            BindingContext = new CreateCardEditorViewModel(model);
        }
    }
}