using Ecco.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public partial class DatabaseManager
    {
        public async Task<List<Template>> GetTemplates()
        {
            var response = await client.GetAsync("api/Templates");
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Template>>(content);
        }

        public async Task<Template> GetTemplate(int id)
        {
            var response = await client.GetAsync("api/Template?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Template>(content);
        }
    }
}
