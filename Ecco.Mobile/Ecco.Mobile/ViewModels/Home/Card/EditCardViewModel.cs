using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
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
            UserData user = JsonConvert.DeserializeObject<UserData>(CrossSettings.Current.GetValueOrDefault("UserData", ""));

            string selectedServiceType = CardModel.ServiceCategory;

            var fields = typeof(ServiceTypes).GetFields();
            int serviceTypeId = 1;
            foreach (var field in fields)
            {
                var serviceInfo = field.GetCustomAttributes(true)[0] as ServiceInfo;
                string title = serviceInfo.Title;
                if (title.Equals(selectedServiceType))
                {
                    int id = (int)field.GetValue(null);
                    serviceTypeId = id;
                    break;
                }
            }

            Entities.Card card = new Entities.Card()
            {
                CardTitle = CardModel.CardTitle,
                Description = CardModel.Description,
                Email = CardModel.Email,
                FullName = CardModel.FullName,
                JobTitle = CardModel.JobTitle,
                Phone = CardModel.PhoneNumber,
                UserId = user.Id,
                TemplateId = SelectedTemplate.Id,
                ServiceType = serviceTypeId,
                Id = _cardId
            };


            var succesful = await _db.EditCard(card);
            if (succesful)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Success", "Changes saved", "Ok");
            }
            else
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered while trying to save your changes. Please try again later.", "Ok");
            }
        }
    }
}