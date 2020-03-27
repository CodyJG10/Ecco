using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities
{
    public class Connection
    {
        public int Id { get; set; }
        public Guid FromId { get; set; }
        public Guid ToId { get; set; }
        public int Status { get; set; }
    }
}
