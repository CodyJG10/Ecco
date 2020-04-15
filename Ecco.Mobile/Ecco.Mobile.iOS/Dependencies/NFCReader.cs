using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public class NFCReader : NSObject, INFCReader, INFCNdefReaderSessionDelegate
    {
        //#region Properties

        //List<NFCNdefMessage> DetectedMessages = new List<NFCNdefMessage> { };
        //NFCNdefReaderSession Session;
        //string CellIdentifier = "reuseIdentifier";

        //#endregion

        //public IntPtr Handle => throw new NotImplementedException();

        //public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        //{
        //    foreach (NFCNdefMessage msg in messages)
        //    { 
        //        DetectedMessages.Add(msg);
        //    }
        //}

        //public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        //{
        //    var readerError = (NFCReaderError)(long)error.Code;
        //    if (readerError != NFCReaderError.ReaderSessionInvalidationErrorFirstNDEFTagRead &&
        //        readerError != NFCReaderError.ReaderSessionInvalidationErrorUserCanceled)
        //    {
        //        Console.WriteLine(readerError);
        //    }
        //}

        //public void Dispose()
        //{
        //}

        //public List<string> ReadTag()
        //{
        //    //var appDelegate = (AppDelegate)UIApplication.SharedApplication.Delegate;
        //    Session = new NFCNdefReaderSession(this, null, true);
        //    Session?.BeginSession();
        //    return new List<string>();
        //    //return null;
        //}

        public string ErrorText { get; private set; }

        private NFCNdefReaderSession _session;
        private TaskCompletionSource<string> _tcs;

        public Task<string> ScanAsync()
        {
            if (!NFCNdefReaderSession.ReadingAvailable)
            {
                throw new InvalidOperationException("Reading NDEF is not available");
            }

            _tcs = new TaskCompletionSource<string>();
            _session = new NFCNdefReaderSession(this, null, true);
            _session.BeginSession();

            return _tcs.Task;
        }

        public void DidInvalidate(NFCNdefReaderSession session, NSError error)
        {
            Console.WriteLine("ServiceToolStandard DidInvalidate: " + error.ToString());
            _tcs.TrySetException(new Exception(error?.LocalizedFailureReason));
        }

        public void DidDetect(NFCNdefReaderSession session, NFCNdefMessage[] messages)
        {
            Console.WriteLine("ServiceToolStandard DidDetect msgs " + messages.Length);
            var bytes = messages[0].Records[0].Payload.Skip(3).ToArray();
            var message = Encoding.UTF8.GetString(bytes);
            Console.WriteLine("ServiceToolStandard DidDetect msg " + message);
            _tcs.SetResult(message);
        }

        public IntPtr Handle { get; }

        public void Dispose()
        {
            Console.WriteLine("ServiceToolStandard Dispose");
        }

        public List<string> ReadTag()
        {
            ScanAsync();
            return null;
        }
    }
}