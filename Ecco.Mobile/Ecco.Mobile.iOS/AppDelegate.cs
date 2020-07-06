using Syncfusion.SfNavigationDrawer.XForms.iOS;
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
using UserNotifications;
using WindowsAzure.Messaging;
using Microsoft.Azure.NotificationHubs;
using Xamarin.Essentials;
using Syncfusion.SfBarcode.iOS;
using Syncfusion.SfBarcode.XForms.iOS;
using Syncfusion.XForms.iOS.BadgeView;
using System.Threading.Tasks;
using Syncfusion.SfImageEditor.XForms.iOS;
using Syncfusion.XForms.iOS.MaskedEdit;

namespace Ecco.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public string PushNotificationsHandle { get; set; }

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            new Syncfusion.SfAutoComplete.XForms.iOS.SfAutoCompleteRenderer();
            Xamarin.Forms.Forms.SetFlags("IndicatorView_Experimental");

            global::Xamarin.Forms.Forms.Init();
            SfNavigationDrawerRenderer.Init();

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
            SfBadgeViewRenderer.Init();
            SfImageEditorRenderer.Init();
            SfMaskedEditRenderer.Init();

            new SfBarcodeRenderer();

            LoadApplication(new App());

            RegisterForRemoteNotifications();

            return base.FinishedLaunching(app, options);
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }

        #region Notifications

        public void RegisterForRemoteNotifications()
        {
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
            if (string.IsNullOrEmpty(oldDeviceToken))
            {

                byte[] bytes = deviceToken.ToArray<byte>();
                string[] hexArray = bytes.Select(b => b.ToString("x2")).ToArray();
                string newDeviceToken = string.Join(string.Empty, hexArray);

                NSUserDefaults.StandardUserDefaults.SetString("PushDeviceToken", newDeviceToken);
                PushNotificationsHandle = newDeviceToken;
            }
            else
            {
                PushNotificationsHandle = oldDeviceToken;
            }
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }
            }
            else
            {
                Console.WriteLine($"Received request to process notification but there was no payload.");
            }
        }

        #endregion

    }
}