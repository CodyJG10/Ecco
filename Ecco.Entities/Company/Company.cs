using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities.Company
{
    public class Company
    {
        public int Id { get; set; }
        public Guid OwnerId { get; set; }
        public string CompanyName { get; set; }
        public string CompanyDescription { get; set; }
    }
}
