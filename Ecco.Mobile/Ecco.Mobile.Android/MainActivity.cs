using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Net;
using Plugin.NFC;
using Android.Content;
using Android;

namespace Ecco.Mobile.Droid
{
    [IntentFilter(new[] { Intent.ActionView },
              Categories = new[] { Intent.CategoryBrowsable, Intent.CategoryDefault },
              DataScheme = "https",
              DataHost = "ecco-space.azurewebsites.net",
              DataPathPrefix = "/cards",
              AutoVerify = true)]
    [Activity(Label = "Ecco.Mobile",
              Icon = "@mipmap/icon", 
              Theme = "@style/MainTheme",
              MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation,
              LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public string DeviceToken { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {   
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            CrossNFC.Init(this);
            
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Syncfusion.XForms.Android.PopupLayout.SfPopupLayoutRenderer.Init();

            LoadApplication(new App());

            CheckAppPermissions();

            CreateNotificationChannel();
        }

        private void CheckAppPermissions()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                return;
            }
            else
            {
                if (PackageManager.CheckPermission(Manifest.Permission.ReadExternalStorage, PackageName) != Permission.Granted
                    && PackageManager.CheckPermission(Manifest.Permission.WriteExternalStorage, PackageName) != Permission.Granted)
                {
                    var permissions = new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage };
                    RequestPermissions(permissions, 1);
                }
            }
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            CrossNFC.OnNewIntent(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void CreateNotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelName = "Ecco";
                var channelDescription = String.Empty;
                var channel = new NotificationChannel(channelName, channelName, NotificationImportance.Default)
                {
                    Description = channelDescription
                };

                var notificationManager = (NotificationManager)GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }
        }
    }
}