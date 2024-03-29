﻿using Ecco.Mobile.ViewModels.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadTagPage : ContentPage
    {
        public ReadTagPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            (BindingContext as ReadTagViewModel).UnsubscribeEvents();
            return base.OnBackButtonPressed();
        }
    }
}