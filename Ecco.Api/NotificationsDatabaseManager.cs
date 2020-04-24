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
        public async Task<string> RegisterDevice()
        {
            var response = await client.GetAsync("notifications/Register");
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<bool> EnablePushNotifications(string id, DeviceRegistration deviceUpdate)
        {
            string json = JsonConvert.SerializeObject(deviceUpdate);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync("notifications/enable/" + id, content);
            return response.IsSuccessStatusCode;
        }
    }
}
