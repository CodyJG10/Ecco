using Syncfusion.XForms.iOS.PopupLayout;
using Syncfusion.SfPullToRefresh.XForms.iOS;
using Syncfusion.XForms.iOS.DataForm;
using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.Expander;
using Syncfusion.XForms.iOS.Expander;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.XForms.iOS.Backdrop;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfPicker.XForms.iOS;
using Syncfusion.XForms.iOS.Shimmer;
using CoreNFC;
using System.Text;

namespace Ecco.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
       // public NFCNdefReaderSession Session { get; set; }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            new Syncfusion.SfAutoComplete.XForms.iOS.SfAutoCompleteRenderer();

            global::Xamarin.Forms.Forms.Init();

            SfButtonRenderer.Init();
            SfExpanderRenderer.Init();
            SfBusyIndicatorRenderer.Init();
            SfBackdropPageRenderer.Init();
            SfComboBoxRenderer.Init();
            SfListViewRenderer.Init();
            SfPickerRenderer.Init();
            SfPopupLayoutRenderer.Init();
            SfPullToRefreshRenderer.Init();
            SfDataFormRenderer.Init();
            SfShimmerRenderer.Init();

            //if (Session == null)
            //{
            //    Session = new NFCNdefReaderSession(this, null, true);
            //}
            //Session = new NFCNdefReaderSession(this, null, true);
            //Session?.BeginSession();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }

        //public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        //{
        //    Console.WriteLine("ServiceToolStandard DidInvalidate: " + error.ToString());
        //}

        //public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        //{
        //    var bytes = messages[0].Records[0].Payload.Skip(3).ToArray();
        //    var message = Encoding.UTF8.GetString(bytes);
        //    Console.WriteLine("ServiceToolStandard DidDetect: " + message);
        //}
    }
}
