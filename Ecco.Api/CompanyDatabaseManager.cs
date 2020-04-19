using System;
using System.Collections.Generic;
using System.Text;
using Ecco.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Ecco.Entities.Company;

namespace Ecco.Api
{
    public partial class DatabaseManager
    {
        public async Task<bool> CreateCompany(Company company)
        {
            string json = JsonConvert.SerializeObject(company);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/CreateCompany", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> InviteEmployee(EmployeeInvitation invitation)
        {
            string json = JsonConvert.SerializeObject(invitation);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/InviteEmployee", httpContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<Company> GetCompany(int id)
        {
            var response = await client.GetAsync("api/Company?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Company>(content);
        }

        public async Task<Company> GetMyOwnedCompany(Guid id)
        {
            var response = await client.GetAsync("api/MyOwnedCompany?userId=" + id.ToString());
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Company>(content);
        }

        public async Task<List<Company>> GetMyEmployers(string userId)
        {
            var response = await client.GetAsync("api/MyEmployers?id=" + userId);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Company>>(content);
        }

        public async Task<bool> LeaveCompany(Company company, Guid userId)
        {
            string json = JsonConvert.SerializeObject(company);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("api/LeaveCompany?userId=" + userId.ToString(), content);
            return response.IsSuccessStatusCode;
        }
    }
}