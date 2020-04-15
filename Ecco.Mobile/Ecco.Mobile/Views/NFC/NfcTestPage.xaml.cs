using Ecco.Api;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.NFC;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcTestPage : ContentPage, INotifyPropertyChanged
    {
        public NfcTestPage()
        {
            InitializeComponent();
        }

		public const string ALERT_TITLE = "NFC";
		public const string MIME_TYPE = "application/com.ecco-space.mobile";

		NFCNdefTypeFormat _type;
		bool _makeReadOnly = false;

		private bool _nfcIsEnabled;
		public bool NfcIsEnabled
		{
			get => _nfcIsEnabled;
			set
			{
				_nfcIsEnabled = value;
				OnPropertyChanged(nameof(NfcIsEnabled));
				OnPropertyChanged(nameof(NfcIsDisabled));
			}
		}

		public bool NfcIsDisabled => !NfcIsEnabled;

		protected override void OnAppearing()
		{
			base.OnAppearing();
			InitNfc();
		}

		private async void InitNfc()
		{
			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{ 
					await ShowAlert("NFC is not available");
				}
				NfcIsEnabled = CrossNFC.Current.IsEnabled;
				if (!NfcIsEnabled)
				{
					await ShowAlert("NFC is disabled");
				}
				SubscribeEvents();

				if (Device.RuntimePlatform != Device.iOS)
				{
					// Start NFC tag listening manually
					CrossNFC.Current.StartListening();
				}
			}
		}

		protected override bool OnBackButtonPressed()
		{
			UnsubscribeEvents();
			CrossNFC.Current.StopListening();
			return base.OnBackButtonPressed();
		}

		void SubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;
			CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
			CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (tagInfo == null)
			{
				await ShowAlert("No tag found");
				return;
			}

			// Customized serial number
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
			var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

			if (!tagInfo.IsSupported)
			{
				await ShowAlert("Unsupported tag (app)", title);
			}
			else if (tagInfo.IsEmpty)
			{
				await ShowAlert("Empty tag", title);
			}
			else
			{
				IDatabaseManager db = TinyIoCContainer.Current.Resolve<IDatabaseManager>() as IDatabaseManager;
				var first = tagInfo.Records[0];
				//string msg = first.Message;
				//string msgHex = NFCUtils.ByteArrayToHexString(tagInfo.Records[0].Payload);
				//string msg = Convert.ToString(msgHex);
				//string msg = NFCUtils.GetMessage(tagInfo.Records[0]);
				//string msg = NFCUtils.GetMessage(tagInfo.Records[0]);
				//string msg = Encoding.UTF8.GetString(first.Payload);
				//string msg2 = GetMessage(first);
				//string msg = Encoding.ASCII.GetString(first.Payload);
				var msgSegments = first.Message.Split('/');
				string msg = msgSegments[msgSegments.Length - 1];
                try
				{
					//byte[] ba = Encoding.Default.GetBytes(msg);
					//var hexString = BitConverter.ToString(ba);
					//hexString = hexString.Replace("-", "");
					//var hexToValue = Convert.ToInt64(hexString, 16);
					//long cardIdMsg = Convert.ToInt64(tagInfo.Records[0].Message, 16);

					//var hexString = NFCUtils.ByteArrayToHexString(first.Payload);
					//int cardId32 = Convert.ToInt32(hexString);
					//long cardId64 = Convert.ToInt64(hexString);

					//tring messageEncoded = Encoding.UTF8.GetString(tagInfo.Records[0].Payload);
					//int cardId = int.Parse(int.Parse(c);
					var card = await db.GetCard(int.Parse(msg));
					//Entities.Card card = JsonConvert.DeserializeObject<Entities.Card>(msg);
					var model = await CardModel.FromCard(card);
					await Application.Current.MainPage.Navigation.PushAsync(new ViewCardPage(model));
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
				}
				//await ShowAlert(GetMessage(first), title);
			}
		}

		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e) => Debug("User has cancelled NFC Session");

		async void Current_OnMessagePublished(ITagInfo tagInfo)
		{
			try
			{
				CrossNFC.Current.StopPublishing();
				if (tagInfo.IsEmpty)
					await ShowAlert("Formatting tag successfully");
				else
					await ShowAlert("Writing tag successfully");
			}
			catch (System.Exception ex)
			{
				await ShowAlert(ex.Message);
			}
		}

		async void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
			if (!CrossNFC.Current.IsWritingTagSupported)
			{
				await ShowAlert("Writing tag is not supported on this device");
				return;
			}

			try
			{
				NFCNdefRecord record = null;
				switch (_type)
				{
					case NFCNdefTypeFormat.WellKnown:
						record = new NFCNdefRecord
						{
							TypeFormat = NFCNdefTypeFormat.WellKnown,
							MimeType = MIME_TYPE,
							Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!")
						};
						break;
					case NFCNdefTypeFormat.Uri:
						record = new NFCNdefRecord
						{
							TypeFormat = NFCNdefTypeFormat.Uri,
							Payload = NFCUtils.EncodeToByteArray("https://github.com/franckbour/Plugin.NFC")
						};
						break;
					case NFCNdefTypeFormat.Mime:
						record = new NFCNdefRecord
						{
							TypeFormat = NFCNdefTypeFormat.Mime,
							MimeType = MIME_TYPE,
							Payload = NFCUtils.EncodeToByteArray("Plugin.NFC is awesome!")
						};
						break;
					default:
						break;
				}

				if (!format && record == null)
					throw new Exception("Record can't be null.");

				tagInfo.Records = new[] { record };

				if (format)
					CrossNFC.Current.ClearMessage(tagInfo);
				else
				{
					CrossNFC.Current.PublishMessage(tagInfo, _makeReadOnly);
				}
			}
			catch (System.Exception ex)
			{
				await ShowAlert(ex.Message);
			}
		}

		async void Button_Clicked_StartListening(object sender, System.EventArgs e)
		{

			try
			{
				CrossNFC.Current.StartListening();
			}
			catch (Exception ex)
			{
				await ShowAlert(ex.Message);
			}
		}

		void Button_Clicked_StartWriting(object sender, System.EventArgs e) => Publish(NFCNdefTypeFormat.WellKnown);

		void Button_Clicked_StartWriting_Uri(object sender, System.EventArgs e) => Publish(NFCNdefTypeFormat.Uri);

		void Button_Clicked_StartWriting_Custom(object sender, System.EventArgs e) => Publish(NFCNdefTypeFormat.Mime);

		void Button_Clicked_FormatTag(object sender, System.EventArgs e) => Publish();

		async void Publish(NFCNdefTypeFormat? type = null)
		{
			try
			{
				if (type.HasValue) _type = type.Value;
				CrossNFC.Current.StartPublishing(!type.HasValue);
			}
			catch (System.Exception ex)
			{
				await ShowAlert(ex.Message);
			}
		}

		string GetMessage(NFCNdefRecord record)
		{
			var message = $"Message: {record.Message}";
			message += Environment.NewLine;
			message += $"RawMessage: {Encoding.UTF8.GetString(record.Payload)}";
			message += Environment.NewLine;
			message += $"Type: {record.TypeFormat.ToString()}";

			if (!string.IsNullOrWhiteSpace(record.MimeType))
			{
				message += Environment.NewLine;
				message += $"MimeType: {record.MimeType}";
			}

			return message;
		}

		void Debug(string message) => System.Diagnostics.Debug.WriteLine(message);

		Task ShowAlert(string message, string title = null) => DisplayAlert(string.IsNullOrWhiteSpace(title) ? ALERT_TITLE : title, message, "Cancel");
	}
}