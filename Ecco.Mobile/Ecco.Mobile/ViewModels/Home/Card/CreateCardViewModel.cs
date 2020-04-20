using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Nancy.TinyIoc;
using Newtonsoft.Json;
using Plugin.Settings;
using Syncfusion.ListView.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using static Ecco.Api.DatabaseManager;

namespace Ecco.Mobile.ViewModels.Home
{
    public class CreateCardViewModel : ViewModelBase
    {
        #region Properties

        private CreateCardModel _createCardModel = new CreateCardModel();
        public CreateCardModel CardModel
        {
            get
            {
                return _createCardModel;
            }
            set
            {
                _createCardModel = value;
                OnPropertyChanged(nameof(CardModel));
            }
        }
        private TemplateModel _selectedTemplate;
        public TemplateModel SelectedTemplate
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

        public bool IsValid { get; set; }

        public ObservableCollection<TemplateModel> Templates { get; set; } = new ObservableCollection<TemplateModel>();

        #endregion

        #region Commands

        public ICommand CreateCommand { get; set; }
        public ICommand TemplateSelectedCommand { get; set; }
        public ICommand ShowServicePickerCommand { get; set; }

        #endregion

        #region Database

        private readonly IDatabaseManager _db;
        private readonly IStorageManager _storage;

        #endregion

        public CreateCardViewModel()
        {
            _db = TinyIoCContainer.Current.Resolve<IDatabaseManager>();
            _storage = TinyIoCContainer.Current.Resolve<IStorageManager>();

            CreateCommand = new Command(CreateCard);
            TemplateSelectedCommand = new Command<TemplateModel>(TemplateSelected);

            LoadTemplates();
        }

        private void TemplateSelected(TemplateModel templateModel)
        {
            SelectedTemplate = templateModel;
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

        public async void CreateCard()
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

            if (SelectedTemplate == null)
                SelectedTemplate = new TemplateModel()
                {
                    Template = new Template()
                    {
                        Id = 1
                    }
                };

            Entities.Card card = new Entities.Card()
            {
                CardTitle = CardModel.CardTitle,
                Description = CardModel.Description,
                Email = CardModel.Email,
                FullName = CardModel.FullName,
                JobTitle = CardModel.JobTitle,
                Phone = CardModel.PhoneNumber,
                UserId = user.Id,
                TemplateId = SelectedTemplate.Template.Id,
                ServiceType = serviceTypeId
            };

            var succeeded = await _db.CreateCard(card);
            if (succeeded)
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "An error was encountered when attempting to create the card.", "Ok");
            }
        }
    }
}