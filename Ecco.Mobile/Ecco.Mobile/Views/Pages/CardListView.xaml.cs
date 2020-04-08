﻿using Ecco.Mobile.ViewModels.Home;
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
    public partial class CardListView : ContentPage
    {
        public CardListView()
        {
            InitializeComponent();
        }

        public void Refresh()
        { 
            (BindingContext as CardListViewModel).RefreshCommand.Execute(null);
        }
    }
}