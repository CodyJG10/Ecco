﻿using Ecco.Api;
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

namespace Ecco.Mobile.ViewModels.NFC
{
    public class WriteTagViewModel : ViewModelBase
    {
		private IDatabaseManager _db;
		private IStorageManager _storage;
		private UserData _user;

		public ICommand WriteToTagCommand { get; set; }
		public ObservableCollection<CardModel> MyCards { get; set; } = new ObservableCollection<CardModel>();
		private bool _loading = true;
		public bool Loading
		{
			get
			{
				return _loading;
			}
			set
			{
				_loading = value;
				OnPropertyChanged(nameof(Loading));
			}
		}
		private CardModel selectedCard;

		public WriteTagViewModel()
		{
			_db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
			_storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
			_user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

			WriteToTagCommand = new Command<CardModel>(WriteToTag);
			LoadCards();
		}

		private async void LoadCards()
		{
			var cards = (await _db.GetMyCards(_user.Id.ToString())).ToList();

			foreach (var card in cards)
			{
				var templateImage = await TemplateUtil.LoadImageSource(card, _db, _storage);
				CardModel model = new CardModel()
				{
					Card = card,
					TemplateImage = templateImage
				};
				MyCards.Add(model);
			}

			Loading = false;
		}

		#region NFC

		public void WriteToTag(CardModel model)
		{
			InitNfc();
			selectedCard = model;
			CrossNFC.Current.StartPublishing(true);
		}

		private void InitNfc()
		{
			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable)
				{
					//await ShowAlert("NFC is not available");
				}

				SubscribeEvents();

				if (Device.RuntimePlatform != Device.iOS)
				{
					CrossNFC.Current.StartListening();
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
					//await ShowAlert("Writing tag successfully");
				}
			}
			catch (Exception ex)
			{
				//await ShowAlert(ex.Message);
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
				var cardId = selectedCard.Card.Id;
				var payload = NFCUtils.EncodeToByteArray("https://ecco-space.azurewebsites.com/cards/" + selectedCard.Card.Id.ToString());
				var record = new NFCNdefRecord
				{
					TypeFormat = NFCNdefTypeFormat.Uri,
					MimeType = "text/plain",
					Payload = payload
				};

				if (!format && record == null)
					throw new Exception("Record can't be null.");

				tagInfo.Records = new[] { record };

				//if (format)
				//{ 
				//	CrossNFC.Current.ClearMessage(tagInfo);
				//}

				CrossNFC.Current.PublishMessage(tagInfo, false);
			}
			catch (System.Exception ex)
			{
				//await ShowAlert(ex.Message);
				return;
			}
		}

		#endregion
	}
}