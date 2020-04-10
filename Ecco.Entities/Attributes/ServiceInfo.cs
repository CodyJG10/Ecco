using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities.Attributes
{
    public class ServiceInfo : Attribute
    {
        public string Title { get; set; }

        public ServiceInfo(string title)
        {
            Title = title;
        }
    }
}