using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities
{
    public class Card
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public string JobTitle { get; set; }
        public string Description { get; set; }
        public string CardTitle { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}