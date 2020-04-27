using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Pages.Cards;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Xamarin.Forms;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class EditCardViewModel : ViewModelBase
    {
        private readonly IDatabaseManager _db;
        private readonly IStorageManager _storage;
        private readonly int _cardId;

        private Template _selectedTemplate;
        public Template SelectedTemplate
        {
            get
            {
                return _selectedTemplate;
            }
            set
            {
                _selectedTemplate = value;
                OnPropertyChanged(nameof(SelectedTemplate));
            }
        }
        public ObservableCollection<TemplateModel> Templates { get; set; } = new ObservableCollection<TemplateModel>();
        public CreateCardModel CardModel { get; set; }
        
        public ICommand SaveCommand { get; set; }
        public ICommand TemplateSelectedCommand { get; set; }

        public EditCardViewModel(CreateCardModel card, int templateId, int cardId)
        {
            _cardId = cardId;
            CardModel = card;

            SelectedTemplate = new Template()
            {
                Id = templateId
            };

            _db = TinyIoCContainer.Current.Resolve(typeof(IDatabaseManager)) as IDatabaseManager;
            _storage = TinyIoCContainer.Current.Resolve(typeof(IStorageManager)) as IStorageManager;

            SaveCommand = new Command(Save);
            TemplateSelectedCommand = new Command<Template>(TemplateSelected);

            LoadTemplates();
        }

        private void TemplateSelected(Template template)
        {
            SelectedTemplate = template;
        }

        private async void LoadTemplates()
        {
            var userData = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            if ((await _db.GetMyEmployers(userData.Id.ToString())).Count > 0)
            {
                var myEmployers = await _db.GetMyEmployers(userData.Id.ToString());
                foreach (var employer in myEmployers)
                {
                    var template = await _db.GetTemplate(employer.TemplateId);
                    var templateImage = await TemplateUtil.LoadImageSource(new Entities.Card() { TemplateId = template.Id }, _db, _storage);
                    TemplateModel templateModel = new TemplateModel()
                    {
                        Template = template,
                        TemplateImage = templateImage
                    };
                    Templates.Add(templateModel);
                }
            }

            var allTemplates = await _db.GetTemplates();
            foreach (var template in allTemplates)
            {
                var templateImage = await TemplateUtil.LoadImageSource(new Entities.Card() { TemplateId = template.Id }, _db, _storage);
                TemplateModel templateModel = new TemplateModel()
                {
                    Template = template,
                    TemplateImage = templateImage
                };
                Templates.Add(templateModel);
            }
        }

        private async void Save()
        {
            TemplateModel templateModel = new TemplateModel()
            {
                Template = SelectedTemplate,
                TemplateImage = await TemplateUtil.LoadImageSource(SelectedTemplate.Id, _db, _storage)
            };
            
            CardModel.TemplateImage = templateModel.TemplateImage;
            CardModel.TemplateId = templateModel.Template.Id;
            CardModel.IsCompanyTemplate = templateModel.Template.IsPublic == false;
            CardModel.id = _cardId;

            var page = new EditCardEditor(CardModel);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}