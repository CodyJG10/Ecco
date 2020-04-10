﻿using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home;
using Syncfusion.XForms.DataForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
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

        private void TemplateListView_ItemTapped(object sender, Syncfusion.ListView.XForms.ItemTappedEventArgs e)
        {
            TemplateExpander.IsExpanded = false;
            TemplateModel model = TemplateListView.SelectedItem as TemplateModel;
            (BindingContext as CreateCardViewModel).TemplateSelectedCommand.Execute(model);
        }
    }
}