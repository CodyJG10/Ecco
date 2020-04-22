using Ecco.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public partial class DatabaseManager
    {
        public async Task<bool> RegisterDevice(DeviceRegistration device)
        {
            string json = JsonConvert.SerializeObject(device);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("notifications/RegisterDevice", content);
            return response.IsSuccessStatusCode;
        }
    }
}
