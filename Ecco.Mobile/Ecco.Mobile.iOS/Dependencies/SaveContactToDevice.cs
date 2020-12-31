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
            //var store = new CNContactStore();
            //var contact = new CNMutableContact();
            //var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(phone));
            //var phoneNumber = new[] { cellPhone };
            //contact.PhoneNumbers = phoneNumber;
            //contact.GivenName = name;
            //var saveRequest = new CNSaveRequest();
            //saveRequest.AddContact(contact, store.DefaultContainerIdentifier);

            // Create a new Mutable Contact (read/write)
            var contact = new CNMutableContact();

            // Set standard properties
            contact.GivenName = name;
            var cellPhone = new CNLabeledValue<CNPhoneNumber>(CNLabelPhoneNumberKey.Mobile, new CNPhoneNumber(phone));
            var numbers = new[] { cellPhone };
            contact.PhoneNumbers = numbers;
            contact.Note = "Added from Ecco Space";

            // Save new contact
            var store = new CNContactStore();
            var saveRequest = new CNSaveRequest();
            saveRequest.AddContact(contact, store.DefaultContainerIdentifier);

            if (store.ExecuteSaveRequest(saveRequest, out NSError error))
            {
                Console.WriteLine("New contact saved");
            }
            else
            {
                Console.WriteLine("Save error: {0}", error);
            }
        }
    }
}