using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.NFC;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;

namespace Ecco.Mobile.ViewModels.NFC
{
    public class WriteTagViewModel : ViewModelBase
    {
		private readonly UserData _user;

		public ICommand WriteToTagCommand { get; set; }

		private bool _android_IsWritingToTag = false;
		public bool Android_IsWritingToTag
		{
			get
			{
				return _android_IsWritingToTag;
			}
			set
			{
				_android_IsWritingToTag = value;
				OnPropertyChanged(nameof(Android_IsWritingToTag));
			}
		}

		public WriteTagViewModel()
		{
			_user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

			WriteToTagCommand = new Command<CardModel>(WriteToTag);
		}

		#region NFC

		public void WriteToTag(CardModel model)
		{
			InitNfc();
			CrossNFC.Current.StartPublishing(true);
		}

		private void InitNfc()
		{
			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{
					Application.Current.MainPage.DisplayAlert("Error", "NFC is not available", "Return");
					Application.Current.MainPage.Navigation.PopAsync();
					return;
				}

				SubscribeEvents();

				if (Device.RuntimePlatform != Device.iOS)
				{
					CrossNFC.Current.StartListening();
					Android_IsWritingToTag = true;
				}
			}
		}

		private void SubscribeEvents()
		{
			CrossNFC.Current.OnMessagePublished += Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered += Current_OnTagDiscovered;

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled += Current_OniOSReadingSessionCancelled;
		}

		public void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessagePublished -= Current_OnMessagePublished;
			CrossNFC.Current.OnTagDiscovered -= Current_OnTagDiscovered;

			if (Device.RuntimePlatform == Device.iOS)
				CrossNFC.Current.OniOSReadingSessionCancelled -= Current_OniOSReadingSessionCancelled;
		}

		void Current_OniOSReadingSessionCancelled(object sender, EventArgs e)
		{
			UnsubscribeEvents();
			Console.WriteLine("Session Cancelled");
		} 

		private void Current_OnMessagePublished(ITagInfo tagInfo)
		{
			try
			{
				Application.Current.MainPage.Navigation.PopAsync();
				CrossNFC.Current.StopPublishing();
				if (tagInfo.IsEmpty)
				{
					//await ShowAlert("Formatting tag successfully");
				}
				else
				{
					Application.Current.MainPage.DisplayAlert("Success", "Wrote to tag succesfully", "Ok");
				}
			}
			catch (Exception ex)
			{
				Application.Current.MainPage.DisplayAlert("Error", "Experienced an error when writing tag: " + ex.Message, "Returning");
				Application.Current.MainPage.Navigation.PopAsync();
				return;
			}
		}

		void Current_OnTagDiscovered(ITagInfo tagInfo, bool format)
		{
			if (!CrossNFC.Current.IsWritingTagSupported)
			{
				return;
			}

			try
			{
				var payload = NFCUtils.EncodeToByteArray("https://ecco-space.azurewebsites.net/usercard/" + _user.Id.ToString());
				var record = new NFCNdefRecord
				{
					TypeFormat = NFCNdefTypeFormat.Uri,
					MimeType = "text/plain",
					Payload = payload
				};

				if (!format && record == null)
					throw new Exception("Record can't be null.");

				tagInfo.Records = new[] { record };

				CrossNFC.Current.PublishMessage(tagInfo, false);
			}
			catch (Exception ex)
			{
				Application.Current.MainPage.DisplayAlert("Error", "Experienced an error when writing tag: " + ex.Message, "Returning");
				Application.Current.MainPage.Navigation.PopAsync();
			}
		}

		#endregion
	}
}