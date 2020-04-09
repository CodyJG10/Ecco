using Ecco.Api;
using Ecco.Entities;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class CreateCardViewModel : ViewModelBase
    {
        public string CardTitle { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        private Template template;

        private List<TemplateModel> _templates;
        public List<TemplateModel> Templates
        {
            get
            {
                return _templates;
            }
            set
            {
                _templates = value;
                OnPropertyChanged(nameof(Templates));
            }
        }

        public ICommand CreateCommand { get; set; }
        public ICommand TemplateSelectedCommand { get; set; }

        private readonly IDatabaseManager _db;
        private readonly IStorageManager _storage;

        public CreateCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();
            
            CreateCommand = new Command(CreateCard);
            TemplateSelectedCommand = new Command<TemplateModel>(TemplateSelected);
           // TemplateSelectedCommand = new Command(TemplateSelected);

            LoadTemplates();
        }

        private void TemplateSelected(TemplateModel templateModel)
        {
            template = templateModel.Template;
        }

        private async void LoadTemplates()
        {
            var allTemplates = await _db.GetTemplates();
            List<TemplateModel> templates = new List<TemplateModel>();
            foreach (var template in allTemplates)
            {
                var templateImage = await TemplateUtil.LoadImageSource(new Entities.Card() { TemplateId = template.Id }, _db, _storage);
                TemplateModel templateModel = new TemplateModel()
                {
                    Template = template,
                    TemplateImage = templateImage
                };
                templates.Add(templateModel);
            }
            Templates = templates;
        }

        public async void CreateCard()
        {
            UserData user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));
            Entities.Card card = new Entities.Card()
            {
                CardTitle = CardTitle,
                Description = Description,
                Email = Email,
                JobTitle = JobTitle,
                Phone = Phone,
                UserId = user.Id,
                TemplateId = template.Id
            };
            var succeeded = await _db.CreateCard(card);
            if (succeeded)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else 
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card. Please ensure all information is filled in", "Ok");
            }
        }
    }
}