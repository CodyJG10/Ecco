﻿using Ecco.Api;
using Ecco.Entities;
using Ecco.Entities.Attributes;
using Ecco.Mobile.Models;
using Ecco.Mobile.Util;
using Ecco.Mobile.Views.Pages.Cards;
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
            if (SelectedTemplate == null)
                SelectedTemplate = new TemplateModel()
                {
                    Template = new Template()
                    {
                        Id = 1
                    }
                };

            CardModel.TemplateImage = SelectedTemplate.TemplateImage;
            CardModel.TemplateId = SelectedTemplate.Template.Id;
            CardModel.IsCompanyTemplate = SelectedTemplate.Template.IsPublic == false;

            var page = new CreateCardEditor(CardModel);
            await Application.Current.MainPage.Navigation.PushAsync(page);
        }
    }
}