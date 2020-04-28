using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Ecco.Mobile.Dependencies;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ecco.Mobile.Droid.Dependencies.SaveContactToDevice))]
namespace Ecco.Mobile.Droid.Dependencies
{
    public class SaveContactToDevice : ISaveContact
    {
        public void SaveContact(string name, string phone, string email)
        {
            var activity = Android.App.Application.Context;
            var intent = new Intent(Intent.ActionInsert);
            intent.SetType(ContactsContract.Contacts.ContentType);
            intent.AddFlags(ActivityFlags.NewTask);
            intent.PutExtra(ContactsContract.Intents.Insert.Name, name);
            intent.PutExtra(ContactsContract.Intents.Insert.Phone, phone);
            intent.PutExtra(ContactsContract.Intents.Insert.Email, email);
            activity.StartActivity(intent);
            Toast.MakeText(activity, "Contact Saved", ToastLength.Short).Show();
        }
    }
}