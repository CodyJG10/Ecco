using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home.Card;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ecco.Mobile.Views.Pages.Cards
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateCardEditor : ContentPage
    {
        private readonly CreateCardModel _cardModel;
        private readonly IStorageManager _storage;
        private readonly UserData _userData;

        private CreateCardEditorViewModel ViewModel
        {
            get 
            {
                return BindingContext as CreateCardEditorViewModel;
            }
        }

        public CreateCardEditor(CreateCardModel model)
        {
            _cardModel = model;
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            InitializeComponent();
            BindingContext = new CreateCardEditorViewModel(model);
            ImageEditor.SetToolbarItemVisibility("path,shape,transform,effects,save", false);

            ImageEditor.ToolbarSettings.ToolbarItems.Add(new HeaderToolbarItem() { Text = "Create" });
            ImageEditor.ToolbarSettings.ToolbarItemSelected += ToolbarSettings_ToolbarItemSelected;
        }

        private async void ToolbarSettings_ToolbarItemSelected(object sender, ToolbarItemSelectedEventArgs e)
        {
            if (e.ToolbarItem.Text == "Create")
            {
                if (ViewModel.Loading) return;

                ViewModel.Loading = true;

                ImageEditor.Save();

                var editStream = ImageEditor.SaveEdits();
                StreamReader reader = new StreamReader(editStream);
                string json = reader.ReadToEnd();

                _cardModel.ExportedImageData = json;

                var imageStream = await ImageEditor.GetStream();

                await _storage.SaveCard(_cardModel.CardTitle, imageStream, _userData.UserName);

                ViewModel.CreateCard();
            }
        }

        private void ImageEditor_ImageSaving(object sender, ImageSavingEventArgs args)
        {
            args.Cancel = true;
        }
    }
}