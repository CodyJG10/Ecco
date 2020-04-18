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
using Newtonsoft.Json.Linq;

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

            InitNotificationsHub();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }

        private void InitNotificationsHub()
        {
            //string connectionString = "Endpoint=sb://ecco-space.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=YO5k7/KyXURG9UpFnMifwvzhjSTTqhT2kOWRko93qlw=";
            // Register for push notifications.
            var settings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert
                | UIUserNotificationType.Badge
                | UIUserNotificationType.Sound,
                new NSSet());

            UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            UIApplication.SharedApplication.RegisterForRemoteNotifications();
        }

        //public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        //{
        //    const string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";

        //    JObject templates = new JObject();
        //    templates["genericMessage"] = new JObject
        //    {
        //    {"body", templateBodyAPNS}
        //    };

        //    // Register for push with your mobile app
        //    Push push = TodoItemManager.DefaultManager.CurrentClient.GetPush();
        //    push.RegisterAsync(deviceToken, templates);
        //}

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            // Connection string from your azure dashboard
            var cs = SBConnectionString.CreateListenAccess(
                new NSUrl("sb://" + HUB_NAME + "-ns.servicebus.windows.net/"),
                HUB_LISTEN_SECRET);

            // Register our info with Azure
            var hub = new SBNotificationHub(cs, HUB_NAME);
            hub.RegisterNative(deviceToken, null, err => {

                if (err != null)
                {
                    Console.WriteLine("Error: " + err.Description);
                    homeViewController.RegisteredForNotifications("Error: " + err.Description);
                }
                else
                {
                    Console.WriteLine("Success");
                    homeViewController.RegisteredForNotifications("Successfully registered for notifications");
                }
            });
        }

        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

            string alert = string.Empty;
            if (aps.ContainsKey(new NSString("alert")))
                alert = (aps[new NSString("alert")] as NSString).ToString();

            //show alert
            if (!string.IsNullOrEmpty(alert))
            {
                UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                avAlert.Show();
            }
        }
    }
}
