using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string CardTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int TemplateId { get; set; }
        public int ServiceType { get; set; }
        public bool IsUsingCompanyTemplate { get; set; }
        public string ExportedImageData { get; set; }
    }
}