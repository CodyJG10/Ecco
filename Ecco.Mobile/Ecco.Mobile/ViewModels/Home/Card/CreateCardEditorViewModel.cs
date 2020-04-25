using Ecco.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.ViewModels.Home.Card
{
    public class CreateCardEditorViewModel : ViewModelBase
    {
        public CardModel Model { get; set; }

        public CreateCardEditorViewModel(CardModel model)
        {
            Model = model;
        }
    }
}