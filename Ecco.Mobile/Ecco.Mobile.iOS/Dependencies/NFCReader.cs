using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CoreFoundation;
using CoreNFC;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.iOS.Dependencies;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(NFCReader))]
namespace Ecco.Mobile.iOS.Dependencies
{
    public class NFCReader : INFCNdefReaderSessionDelegate, INFCReader
    {
        #region Properties

        List<NFCNdefMessage> DetectedMessages = new List<NFCNdefMessage> { };
        NFCNdefReaderSession Session;
        string CellIdentifier = "reuseIdentifier";

        #endregion

        public IntPtr Handle => throw new NotImplementedException();

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            foreach (NFCNdefMessage msg in messages)
            { 
                DetectedMessages.Add(msg);
            }
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            var readerError = (NFCReaderError)(long)error.Code;
            if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
                readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
            {
                Console.WriteLine(readerError);
            }
        }

        public void Dispose()
        {
        }

        public List<string> ReadTag()
        {
            var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
            //Session = new NFCNdefReaderSession(appDelegate, null, true);
            //Session?.BeginSession();
            //return new List<string>();
            return null;
        }
    }
}