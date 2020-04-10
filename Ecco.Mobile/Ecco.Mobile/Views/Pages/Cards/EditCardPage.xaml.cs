using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Ecco.Mobile.ViewModels.Home.Card;
using Syncfusion.XForms.DataForm;
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
    public partial class EditCardPage : ContentPage
    {
        public EditCardPage(CreateCardModel card, int templateId, int cardId)
        {
            InitializeComponent();
            BindingContext = new EditCardViewModel(card, templateId, cardId);
        }

        private void DataForm_AutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
        {
            DataForm.RegisterEditor("ServiceCategory", "DropDown");

            if (e.DataFormItem != null && e.DataFormItem.Name == "ServiceCategory")
            {
                var list = new List<string>();
                var fields = typeof(ServiceTypes).GetFields();
                foreach (var field in fields)
                {
                    list.Add((field.GetCustomAttributes(true)[0] as ServiceInfo).Title);
                }
                (e.DataFormItem as DataFormDropDownItem).ItemsSource = list;
            }

            else if (e.DataFormItem != null && e.DataFormItem.Name == "Email")
                (e.DataFormItem as DataFormTextItem).KeyBoard = Keyboard.Email;
        }

        private void TemplateListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            var model = TemplateListView.SelectedItem as TemplateModel;
            TemplateExpander.IsExpanded = false;
            (BindingContext as EditCardViewModel).TemplateSelectedCommand.Execute(model.Template);
        }
    }
}