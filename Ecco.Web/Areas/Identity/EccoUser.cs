using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Areas.Identity
{
    public class EccoUser : IdentityUser
    {
        public string ProfileName { get; set; }
        public int PushNotificationProvider { get; set; }
        public string RefreshToken { get; set; }
        public string ActiveCard { get; set; }
    }
}
