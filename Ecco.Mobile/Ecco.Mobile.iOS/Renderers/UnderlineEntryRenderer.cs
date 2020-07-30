using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreAnimation;
using Ecco.Mobile.Controls;
using Ecco.Mobile.iOS.Renderers;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(UnderlineEntry), typeof(UnderlineEntryRenderer))]
namespace Ecco.Mobile.iOS.Renderers
{
    public class UnderlineEntryRenderer : EntryRenderer
    {
		//protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		//{
		//    base.OnElementChanged(e);

		//    if (this.Control == null) return;

		//    this.Control.BorderStyle = UITextBorderStyle.Bezel;
		//}
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.BorderStyle = UITextBorderStyle.None;

				var view = (Element as UnderlineEntry);
				if (view != null)
				{
					DrawBorder(view);
					SetFontSize(view);
					SetPlaceholderTextColor(view);
				}
			}
		}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

			var view = (UnderlineEntry)Element;

			if (e.PropertyName.Equals(Color.Black))
				DrawBorder(view);
			if (e.PropertyName.Equals(view.FontSize))
				SetFontSize(view);
			if (e.PropertyName.Equals(view.PlaceholderColor))
				SetPlaceholderTextColor(view);
		}

		void DrawBorder(UnderlineEntry view)
		{
			var borderLayer = new CALayer();
			borderLayer.MasksToBounds = true;
			borderLayer.Frame = new CoreGraphics.CGRect(0f, Frame.Height / 2, Frame.Width, 1f);
			borderLayer.BorderColor = Color.Black.ToCGColor();
			borderLayer.BorderWidth = 1.0f;

			Control.Layer.AddSublayer(borderLayer);
			Control.BorderStyle = UITextBorderStyle.None;
		}

		void SetFontSize(UnderlineEntry view)
		{
			if (view.FontSize != Font.Default.FontSize)
				Control.Font = UIFont.SystemFontOfSize((System.nfloat)view.FontSize);
			else if (view.FontSize == Font.Default.FontSize)
				Control.Font = UIFont.SystemFontOfSize(17f);
		}

		void SetPlaceholderTextColor(UnderlineEntry view)
		{
			if (string.IsNullOrEmpty(view.Placeholder) == false && view.PlaceholderColor != Color.Default)
			{
				var placeholderString = new NSAttributedString(view.Placeholder,
											new UIStringAttributes { ForegroundColor = view.PlaceholderColor.ToUIColor() });
				Control.AttributedPlaceholder = placeholderString;
			}
		}
	}
}