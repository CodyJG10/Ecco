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
    }
}
