using Ecco.Entities.CardText;
using Ecco.Mobile.Models;
using Ecco.Mobile.ViewModels.Home.Card;
using Newtonsoft.Json;
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
        public CreateCardEditor(CardModel model)
        {
            InitializeComponent();
            BindingContext = new CreateCardEditorViewModel(model);
            ImageEditor.SetToolbarItemVisibility("path,shape,transform,effects", false);

            ImageEditor.ToolbarSettings.ToolbarItems.Add(new HeaderToolbarItem() { Text = "Export" });
            ImageEditor.ToolbarSettings.ToolbarItemSelected += ToolbarSettings_ToolbarItemSelected; ;
        }

        private void ToolbarSettings_ToolbarItemSelected(object sender, ToolbarItemSelectedEventArgs e)
        {
            if (e.ToolbarItem.Text == "Export")
            {
                var saveStream = ImageEditor.SaveEdits();
                StreamReader reader = new StreamReader(saveStream);
                string json = reader.ReadToEnd();

                TextObjectList list = TextObjectList.FromJson(json);

                Console.WriteLine(json);
            }
        }

        private void ImageEditor_ImageSaved(object sender, Syncfusion.SfImageEditor.XForms.ImageSavedEventArgs args)
        {
            
        }

        private void ImageEditor_ImageEdited(object sender, Syncfusion.SfImageEditor.XForms.ImageEditedEventArgs e)
        {
            if(e.IsImageEdited)
            {
                
            }
        }

        private void ImageEditor_ItemSelected(object sender, ItemSelectedEventArgs args)
        {
          
        }
    }
}