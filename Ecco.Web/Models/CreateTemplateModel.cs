using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Models
{
    public class CreateTemplateModel
    {
        public IFormFile File { get; set; }
        public string Title { get; set; }
    }
}
