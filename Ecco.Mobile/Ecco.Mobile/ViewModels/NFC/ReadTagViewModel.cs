using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.NFC;
using Plugin.Settings;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.NFC
{
    public class ReadTagViewModel : ViewModelBase
    {
		private IDatabaseManager _db;
		private UserData _userData;

		public ReadTagViewModel()
		{
			_db = TinyIoCContainer.Current.Resolve<IDatabaseManager>() as IDatabaseManager;
			_userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

			InitNfc();
		}

		private void InitNfc()
		{
			if (CrossNFC.IsSupported)
			{
				if (!CrossNFC.Current.IsAvailable || !CrossNFC.Current.IsEnabled)
				{
					PopAndShowError("NFC is not available on your device");
				}

				SubscribeEvents();
				CrossNFC.Current.StartListening();
			}
		}

		void SubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived += Current_OnMessageReceived;

		}

		public void UnsubscribeEvents()
		{
			CrossNFC.Current.OnMessageReceived -= Current_OnMessageReceived;
		}

		private async void Current_OnMessageReceived(ITagInfo tagInfo)
		{
			if (tagInfo == null)
			{
				PopAndShowError("No card found");
				return;
			}

			if (!tagInfo.IsSupported)
			{
				PopAndShowError("Unsupported card");
			}
			else if (tagInfo.IsEmpty)
			{
				PopAndShowError("The card that was read was empty");
			}
			else
			{
				var first = tagInfo.Records[0];
				var msgSegments = first.Message.Split('/');
				string msg = msgSegments[msgSegments.Length - 1];
				try
				{
					var card = await _db.GetCard(int.Parse(msg));
					var model = await CardModel.FromCard(card);

					//If the user does not have this card added we display a modal of the card
					if (!await UserHasCard(model))
					{
						await Application.Current.MainPage.Navigation.PushAsync(new CreateConnectionFromScanPage(model));
					}
					else
					{ 
						await Application.Current.MainPage.Navigation.PushAsync(new ViewCardPage(model));
					}
				}
				catch (Exception e)
				{
					PopAndShowError(e.Message);
				}
			}
		}

		private async Task<bool> UserHasCard(CardModel card)
		{
			var connections = await _db.GetMyConnections(_userData.Id);
			foreach (var connection in connections)
			{
				if (connection.CardId == card.Card.Id)
				{
					return true;
				}
			}
			return false;
		}

		private void PopAndShowError(string message)
		{
			Application.Current.MainPage.Navigation.PopAsync();
			Application.Current.MainPage.DisplayAlert("Error", message, "Ok");
		}
	}
}