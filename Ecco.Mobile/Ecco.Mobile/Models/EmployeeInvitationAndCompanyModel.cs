using Ecco.Entities.Company;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Models
{
    public class EmployeeInvitationAndCompanyModel
    {
        public EmployeeInvitation EmployeeInvitation { get; set; }
        public Company Company { get; set; }
    }
}