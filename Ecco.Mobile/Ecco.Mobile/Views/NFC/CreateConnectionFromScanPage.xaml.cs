﻿using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.NFC;
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
    public partial class CreateConnectionFromScanPage : ContentPage
    {
        public CreateConnectionFromScanPage(CardModel model)
        {
            InitializeComponent();
            BindingContext = new CreateConnectionFromScanPageViewModel(model);
        }
    }
}