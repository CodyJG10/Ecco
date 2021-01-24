using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.AutoUpdate;
using Ecco.Mobile.Dependencies;
using Ecco.Mobile.Models;
using Ecco.Mobile.Views;
using Ecco.Mobile.Views.Authentication;
using Ecco.Mobile.Views.HomeMaster;
using Ecco.Mobile.Views.NFC;
using Ecco.Mobile.Views.Pages;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.Diagnostics;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Syncfusion.XForms.Themes;
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
        private bool openEccoCard = false;
        private bool isLaunched = false;
        private string[] eccoCardContent;

        public App()
        {
            InitializeComponent();
            string syncfusionLicense = Mobile.Properties.Resources.SyncfusionLicense;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncfusionLicense);
            MainPage = new LoadingPage();
        }

        #region iOS Deep Linking

        protected override void OnAppLinkRequestReceived(Uri uri)
        {
            base.OnAppLinkRequestReceived(uri);

            eccoCardContent = uri.Segments;

            if (isLaunched)
            {
                ShowFromEccoCard();
            }
            else
            {
                openEccoCard = true;
            }
        }

        private async void ShowFromEccoCard()
        {
            var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            string cardId = null;
            string prefix = eccoCardContent[eccoCardContent.Length - 2];

            if (prefix.ToLower().Contains("usercard"))
            {
                string profileId = eccoCardContent[eccoCardContent.Length - 1];
                var activeCard = await db.GetActiveCard(profileId);
                cardId = activeCard.Id.ToString();
            }
            else if (prefix.ToLower().Contains("cards"))
            {
                cardId = eccoCardContent[eccoCardContent.Length - 1];
            }

            var card = await db.GetCard(int.Parse(cardId));
            var cardToOpen = CardModel.FromCard(card, await db.GetUserData(card.UserId));

            var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
           
            //If the user does not have this card added we display a modal of the card
            if (!await UserHasCard(cardToOpen, userData.Id))
            {
                // If this is a new card, we show the create conection from scan page
                await Current.MainPage.Navigation.PushAsync(new CreateConnectionFromScanPage(cardToOpen));
            }
            else
            {
                // If the card is already in the users card list, we just show the card
                await Current.MainPage.Navigation.PushAsync(new ViewCardPage(cardToOpen));
            }

            isLaunched = true;
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

        private void InitServices()
        {
            var container = TinyIoCContainer.Current;
            container.Register<IDatabaseManager, DatabaseManager>();
            container.Resolve<IDatabaseManager>().SetUrl(Mobile.Properties.Resources.WebUrl);
            container.Register<IStorageManager>(new StorageManager(Mobile.Properties.Resources.StorageConnectionString));
        }

        protected override void OnStart()
        {
            InitServices();
            Authenticate();
        }

        private async void Authenticate()
        {
            var db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            string secret = Mobile.Properties.Resources.WebSecret;
            var result = await db.Authenticate(secret);
            if (!result.IsSuccessStatusCode)
            {
                await Current.MainPage.DisplayAlert("Authentication Error", "Could not authenticate with Ecco Space servers. Please try again later.", "Return");
            }
            else
            {
                if (CrossSettings.Current.Contains("Username"))
                {
                    AutoLogin();
                }
                else
                {
                    MainPage = new LoginPage();
                }
            }
        }

        private void AutoLogin() 
        {
            if (openEccoCard)
            {
                MainPage = new NavigationPage(new HomeMaster())
                {
                    BarBackgroundColor = (Color)Resources["LightRed"],
                    BarTextColor = Color.White,
                };
                ShowFromEccoCard();
                return;
            }
            else
            {
                MainPage = new NavigationPage(new HomeMaster())
                {
                    BarBackgroundColor = (Color)Resources["LightRed"],
                    BarTextColor = Color.White
                };
                isLaunched = true;
            }
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            (TinyIoCContainer.Current.Resolve<AutoUpdater>()).Stop();
        }

        protected override void OnResume()
        {
            if (!(TinyIoCContainer.Current.Resolve<IDatabaseManager>()).TokenIsValid())
            {
                MainPage = new LoadingPage();
                AutoLogin();
            }
            base.OnResume();
            (TinyIoCContainer.Current.Resolve<AutoUpdater>()).Start();
        }
    }
}