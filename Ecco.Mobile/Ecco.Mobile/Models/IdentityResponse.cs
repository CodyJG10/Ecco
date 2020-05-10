using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecco.Mobile.Models
{
    public class IdentityResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; }
        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }
    }
}