using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities.Company
{
    public class EmployeeInvitation
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Status { get; set; }
    }
}