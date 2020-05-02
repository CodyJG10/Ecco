using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contacts;
using Ecco.Mobile.Dependencies;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(Ecco.Mobile.iOS.Dependencies.SaveContactToDevice))]
namespace Ecco.Mobile.iOS.Dependencies
{
    public class SaveContactToDevice : ISaveContact
    {
        public void SaveContact(string name, string phone, string email)
        {
            var store = new CNContactStore();
            var contact = new CNMutableContact();
            var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(phone));
            var phoneNumber = new[] { cellPhone };
            contact.PhoneNumbers = phoneNumber;
            contact.GivenName = name;
            var saveRequest = new CNSaveRequest();
            saveRequest.AddContact(contact, store.DefaultContainerIdentifier);
        }
    }
}