using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Entities
{
    public class UserData
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string ProfileName { get; set; }
    }
}