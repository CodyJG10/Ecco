using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities.Events
{
    public class UpdateEvent
    {
        public string User { get; set; }
        public string Message { get; set; }
    }
}