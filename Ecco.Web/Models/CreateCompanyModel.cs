]using Ecco.Entities.Company;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Models
{
    public class CreateCompanyModel
    {
        public Company Company { get; set; }
        public IFormFile File { get; set; }
    }
}