using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Ecco.Mobile.Droid.Notifications
{
    //[Service]
    //public class BackgroundNotificationService : Service
    //{
    //    public override IBinder OnBind(Intent intent)
    //    {
    //        base.OnBind(intent);
    //    }

    //    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    //    {
    //        // Code omitted for clarity - here is where the service would do something.

    //        // Work has finished, now dispatch anotification to let the user know.
    //        Notification.Builder notificationBuilder = new Notification.Builder(this)
    //            .SetSmallIcon(Resource.Drawable.ic_notification_small_icon)
    //            .SetContentTitle(Resources.GetString(Resource.String.notification_content_title))
    //            .SetContentText(Resources.GetString(Resource.String.notification_content_text));

    //        var notificationManager = (NotificationManager)GetSystemService(NotificationService);
    //        notificationManager.Notify(NOTIFICATION_ID, notificationBuilder.Build());
    //    }
    //}
}