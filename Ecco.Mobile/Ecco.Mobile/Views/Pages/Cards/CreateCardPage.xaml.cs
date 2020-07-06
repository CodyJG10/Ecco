using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Nancy.ViewEngines.SuperSimpleViewEngine;
using Syncfusion.ListView.XForms;
using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using Syncfusion.XForms.MaskedEdit;
using Syncfusion.XForms.PopupLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using static Ecco.Mobile.Models.CreateCardModel;

namespace Ecco.Mobile.Views.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCardPage : ContentPage
    {
        public CreateCardPage()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.iOS)
            { 
                DataForm.LayoutManager = new DataFormLayoutManagerExt(DataForm);
            }
        }

        private void DataForm_AutoGeneratingDataFormItem(object sender, AutoGeneratingDataFormItemEventArgs e)
        {
            DataForm.RegisterEditor("ServiceCategory", "DropDown");

            if (Device.RuntimePlatform == Device.iOS)
            {
                //DataForm.RegisterEditor("Text", new CustomTextEditor(DataForm));
                //DataForm.RegisterEditor("MaskedEditText", new CustomMaskedEditor(DataForm));
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

            else if (e.DataFormItem != null && e.DataFormItem.Name == "Your Email")
                (e.DataFormItem as DataFormTextItem).KeyBoard = Keyboard.Email;

            else if (e.DataFormItem.Name == "Your Phone Number")
            {
                ((DataFormTextItem)e.DataFormItem).KeyBoard = Keyboard.Numeric;
            }
        }

        private void TemplateListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            var model = e.AddedItems[0] as TemplateModel;
            (PopupLayout.BindingContext as CreateCardViewModel).TemplateSelectedCommand.Execute(model);
            (PopupLayout.BindingContext as CreateCardViewModel).CreateCommand.Execute(null);
        }

        private void ButtonBeginEditing_Clicked(object sender, EventArgs e)
        {
            PopupLayout.Show();
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
            view.TextColor = Color.Black;
            view.ErrorBorderColor = Color.Red;
            view.BorderColor = Color.Black;
        }
    }

    #endregion
}