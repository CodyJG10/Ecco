﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Authentication
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailEntry : ContentView
    {
        public EmailEntry()
        {
            InitializeComponent();
        }

        public string GetEmail()
        {
            return Email.Text;
        }
    }
}