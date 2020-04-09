using Ecco.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Models
{
    public class ConnectionModel
    {
        public Connection Connection { get; set; }
        public string Name { get; set; }
        public CardModel Card { get; set; }
    }
}