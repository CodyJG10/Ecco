using Ecco.Entities.Company;
using Ecco.Web.Areas.Identity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Models
{
    public class CreateCompanyModel
    {
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
        public string OwnerEmail { get; set; }
        public IFormFile File { get; set; }
    }
}