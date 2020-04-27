using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Mobile.Util
{
    public static class HttpResponseFormatter
    {
        public static async Task<string> GetErrorFromResponse(HttpResponseMessage response)
        {
            string content = await response.Content.ReadAsStringAsync();
            ResponseMessage message = JsonConvert.DeserializeObject<ResponseMessage>(content);

            if (message.Errors == null)
            {
                return "";
            }
            else 
            {
                return message.Errors[0];
            }
        }

        public struct ResponseMessage
        { 
            [JsonProperty("")]
            public List<string> Errors { get; set; }
        }
    }
}