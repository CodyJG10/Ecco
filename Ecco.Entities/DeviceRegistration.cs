using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities
{
    public class DeviceRegistration
    {
        public string InstallationId { get; set; }
        public string Username { get; set; }
        public string Platform { get; set; }
        public string PushChannel { get; set; }
        public string[] Tags { get; set; }
    }
}