using Plugin.NFC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.NFC
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NfcTestPage : ContentPage
    {
        public NfcTestPage()
        {
            InitializeComponent();
        }

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
					await Application.Current.MainPage.DisplayAlert("NFC", "NFC is not available", "Ok");

				if (!CrossNFC.Current.IsEnabled)
					await Application.Current.MainPage.DisplayAlert("NFC", "NFC is disabled", "Ok");

				SubscribeEvents();

				if (Device.RuntimePlatform != Device.iOS)
				{
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

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (tagInfo == null)
			{
				await Application.Current.MainPage.DisplayAlert("NFC", "No tag found", "Ok");
				return;
			}

			// Customized serial number
			var identifier = tagInfo.Identifier;
			var serialNumber = NFCUtils.ByteArrayToHexString(identifier, ":");
			var title = !string.IsNullOrWhiteSpace(serialNumber) ? $"Tag [{serialNumber}]" : "Tag Info";

			if (!tagInfo.IsSupported)
			{
				await Application.Current.MainPage.DisplayAlert("NFC", "Unsupported Tag", "Ok");
			}
			else if (tagInfo.IsEmpty)
			{
				await Application.Current.MainPage.DisplayAlert("NFC", "Empty Tag", "Ok");
			}
			else
			{
				var first = tagInfo.Records[0];
				await Application.Current.MainPage.DisplayAlert("NFC", GetMessage(first), "Ok");
			}
		}

		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e) => Application.Current.MainPage.DisplayAlert("NFC", "User has cancelled NFC Session", "Ok");

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

		private void ButtonStartListening_Clicked(object sender, EventArgs e)
		{
			try
			{
				CrossNFC.Current.StartListening();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}
	}
}