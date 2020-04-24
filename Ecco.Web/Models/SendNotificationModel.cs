using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Models
{
    public class SendNotificationModel
    {
        [DisplayName("Message")]
        public string Message { get; set; }
        [DisplayName("Username")]
        public string Username { get; set; }
    }
}