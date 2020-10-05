using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Dependencies;
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
    public partial class EditCardEditor : ContentPage
    {
        private readonly CreateCardModel _cardModel;
        private readonly IStorageManager _storage;
        private readonly UserData _userData;

        private EditCardEditorViewModel ViewModel
        {
            get
            {
                return BindingContext as EditCardEditorViewModel;
            }
        }

        public EditCardEditor(CreateCardModel model)
        {
            InitializeComponent();

            _cardModel = model;
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            _userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            BindingContext = new EditCardEditorViewModel(model);
            //ImageEditor.SetToolbarItemVisibility("path,shape,transform,effects,save", false);

            ImageEditor.SetToolbarItemVisibility("path,transform,save", false);

            ImageEditor.ToolbarSettings.ToolbarItems.Add(new HeaderToolbarItem() { Text = "Update" });
            ImageEditor.ToolbarSettings.ToolbarItemSelected += ToolbarSettings_ToolbarItemSelected;

            ImageEditor.ToolbarSettings.ToolbarItems.Add(new FooterToolbarItem()
            {
                Name = "Add Image",
                Icon = ImageSource.FromFile("photo.png")
            });
        }

        private void LoadText()
        {
            string json = _cardModel.ExportedImageData;
            byte[] bytes = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(bytes);
            ImageEditor.LoadEdits(stream);
        }

        private async void ToolbarSettings_ToolbarItemSelected(object sender, ToolbarItemSelectedEventArgs e)
        {
            if (e.ToolbarItem.Text == "Update")
            {
                if (ViewModel.Loading) return;

                ViewModel.Loading = true;

                ImageEditor.Save(".png");

                var editStream = ImageEditor.SaveEdits();
                StreamReader reader = new StreamReader(editStream);
                string json = reader.ReadToEnd();

                _cardModel.ExportedImageData = json;

                var imageStream = await ImageEditor.GetStream();

                await _storage.SaveCard(_cardModel.CardTitle, imageStream, _userData.UserName);

                ViewModel.SaveCard();
            }
            else if (e.ToolbarItem.Name == "Add Image")
            {
                var image = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
                AddCustomImage(image);
            }
        }

        private void ImageEditor_ImageLoaded(object sender, ImageLoadedEventArgs args)
        {
            LoadText();
        }

        private void AddCustomImage(Stream imageStream)
        {
            Image img = new Image
            {
                HeightRequest = 200,
                WidthRequest = 200,
                Source = ImageSource.FromStream(() => imageStream)
            };
            ImageEditor.AddCustomView(img);
        }
    }
}