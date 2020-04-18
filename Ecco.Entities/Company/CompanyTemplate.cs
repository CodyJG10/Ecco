using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities.Company
{
    public class CompanyTemplate
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Filename { get; set; }
    }
}