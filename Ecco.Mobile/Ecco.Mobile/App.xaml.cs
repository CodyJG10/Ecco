using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views;
using Ecco.Mobile.Views.Authentication;
using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.Diagnostics;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MjM1NDA2QDMxMzgyZTMxMmUzMGZGWjB2Z0prb1hpcCtLTDZPVUo3bXhnUGFnV0VRcTZCUEk3UXZZdFB5M1U9");

            InitDatabase();
            
            MainPage = new LoadingPage();

            if (!CrossSettings.Current.GetValueOrDefault("RefreshToken", "_").Equals("_"))
            {
                RefreshToken();
            }
            else 
            {
                MainPage = new LoginPage();
            }
        }

        private async void RefreshToken()
        {
            string refreshToken = CrossSettings.Current.GetValueOrDefault("RefreshToken", "_");
            string token = CrossSettings.Current.GetValueOrDefault("Token", "_");
            var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            var response = await db.RefreshToken(token, refreshToken);
            if (response.IsSuccessStatusCode)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var content = JsonConvert.DeserializeObject<IdentityResponse>(contentString);
                var newToken = content.Token;
                var newRefreshToken = content.RefreshToken;
                db.SetToken(token);
                CrossSettings.Current.AddOrUpdateValue("RefreshToken", newRefreshToken);
                CrossSettings.Current.AddOrUpdateValue("Token", newToken);
                MainPage = new NavigationPage(new Home());
            }
            else 
            {
                MainPage = new LoginPage();
                await MainPage.DisplayAlert("Authentication Error", "You have been logged out", "Ok");
            }
        }

        #region iOS Deep Linking

        protected override async void OnAppLinkRequestReceived(Uri uri)
        {
            // App opened through Universal App Link
            base.OnAppLinkRequestReceived(uri);

            //string cardId = uri.Segments[uri.Segments.Length - 1];
            string cardId;

            if (uri.Segments.Contains("usercard"))
            {
                cardId = null;
            }
            else 
            {
                cardId = uri.Segments[uri.Segments.Length - 1];
            }

            // Check if a user is logged in
            if (!CrossSettings.Current.GetValueOrDefault("Username", "_").Equals("_"))
            {
                // Auto login user
                string username = CrossSettings.Current.GetValueOrDefault("Username", "");
                string password = CrossSettings.Current.GetValueOrDefault("Password", "");
                var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
                var loginResponse = await db.Login(username, password);
                if (loginResponse.IsSuccessStatusCode)
                {
                    // First, navigate to home page
                    MainPage = new NavigationPage(new Home());
                    var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

                    //if user profile, get active card
                    if (cardId == null)
                    {
                        string profileId = uri.Segments[uri.Segments.Length - 1];
                        var activeCard = await db.GetActiveCard(profileId);
                        cardId = activeCard.Id.ToString();
                    }

                    // Show card
                    var card = await db.GetCard(int.Parse(cardId));
                    var model = CardModel.FromCard(card, await db.GetUserData(card.UserId));

                    //If the user does not have this card added we display a modal of the card
                    if (!await UserHasCard(model, userData.Id))
                    {
                        // If this is a new card, we show the create conection from scan page
                        await Current.MainPage.Navigation.PushAsync(new CreateConnectionFromScanPage(model));
                    }
                    else
                    {
                        // If the card is already in the users card list, we just show the card
                        await Current.MainPage.Navigation.PushAsync(new ViewCardPage(model));
                    }
                }
                else
                {
                    // There was an error auto logging in
                    MainPage = new LoginPage();
                    await MainPage.DisplayAlert("Authentication Error", "You have been logged out", "Ok");
                }
            }
        }

        private async Task<bool> UserHasCard(CardModel card, Guid userId)
        {
            var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            var connections = await db.GetMyConnections(userId);
            foreach (var connection in connections)
            {
                if (connection.CardId == card.Card.Id)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        private void InitDatabase()
        {
            var container = TinyIoCContainer.Current;
            container.Register<IDatabaseManager, DatabaseManager>();
            container.Resolve<IDatabaseManager>().SetUrl("https://ecco-space.azurewebsites.net/");
            container.Register<IStorageManager>(new StorageManager("DefaultEndpointsProtocol=https;AccountName=eccospacestorage;AccountKey=Nr6eERil/QqRitQ/XThQ9yElPlH844fwAqE0LDOX6ktyYae0S5xtvv5W/d0lrM3Y7uI8KP7qRgoQ/unHCmYnIw==;EndpointSuffix=core.windows.net"));
        }

        protected override void OnStart()
        {
        }
        
        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
