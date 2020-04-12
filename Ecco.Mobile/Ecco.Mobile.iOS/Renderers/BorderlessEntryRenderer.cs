using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ecco.Mobile.Controls;
using Ecco.Mobile.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(BorderlessEntry), typeof(BorderlessEntryRenderer))]
namespace Ecco.Mobile.iOS.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null) return;

            this.Control.BorderStyle = UITextBorderStyle.None;
        }
    }
}