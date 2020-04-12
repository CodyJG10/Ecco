using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Ecco.Mobile.ViewModels.Home.Card;
using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using Syncfusion.XForms.MaskedEdit;
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

            if (Device.RuntimePlatform == Device.iOS)
            {
                DataForm.RegisterEditor("MultilineText", new CustomMultilineTextEditor(DataForm));
                DataForm.RegisterEditor("Text", new CustomTextEditor(DataForm));
                DataForm.RegisterEditor("MaskedEditText", new CustomMaskedEditor(DataForm));
            }

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

    #region Custom Layout

    public class DataFormLayoutManagerExt : DataFormLayoutManager
    {
        public DataFormLayoutManagerExt(SfDataForm dataForm) : base(dataForm) { }

        protected override View GenerateViewForLabel(DataFormItem dataFormItem)
        {
            var view = base.GenerateViewForLabel(dataFormItem);
            var textView = (view as Label);
            textView.TextColor = Color.Black;
            return view;
        }
    }

    #endregion

    #region Custom Editors

    public class CustomMultilineTextEditor : DataFormMultiLineTextEditor
    {
        public CustomMultilineTextEditor(SfDataForm dataForm) : base(dataForm) { }
        protected override void OnInitializeView(DataFormItem dataFormItem, Editor view)
        {
            base.OnInitializeView(dataFormItem, view);
            view.TextColor = Color.Black;
            view.BackgroundColor = Color.White;
            view.PlaceholderColor = Color.Black;
        }
    }

    public class CustomTextEditor : DataFormTextEditor
    {
        public CustomTextEditor(SfDataForm dataForm) : base(dataForm) { }

        protected override void OnInitializeView(DataFormItem dataFormItem, Entry view)
        {
            base.OnInitializeView(dataFormItem, view);
            view.TextColor = Color.Black;
            view.PlaceholderColor = Color.Black;
        }
    }

    public class CustomMaskedEditor : DataFormMaskedEditTextEditor
    {
        public CustomMaskedEditor(SfDataForm dataForm) : base(dataForm) { }

        protected override void OnInitializeView(DataFormItem dataFormItem, SfMaskedEdit view)
        {
            base.OnInitializeView(dataFormItem, view);
            view.Keyboard = Keyboard.Numeric;
            view.TextColor = Color.Black;
            view.BorderColor = Color.Black;
            view.ErrorBorderColor = Color.Red;
            view.BackgroundColor = Color.LightBlue;
        }
    }

    #endregion
}