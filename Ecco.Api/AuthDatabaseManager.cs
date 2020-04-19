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
        #region Models

        public struct LoginResult
        {
            public string token { get; set; }
        }

        #endregion

        #region Auth

        public async Task<bool> Login(string username, string password)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", username),
                new KeyValuePair<string, string>("Password", password)
            });

            var response = await client.PostAsync("auth/token", formContent);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                LoginResult content = JsonConvert.DeserializeObject<LoginResult>(result);
                string token = content.token;
                SetToken(token);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetToken(string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<bool> Register(string username, string email, string password, string confirmPassword)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("ConfirmPassword", confirmPassword)
            });
            var response = await client.PostAsync("auth/register", formContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<UserData> GetUserData()
        {
            var response = await client.GetAsync("api/UserInfo");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<UserData> GetUserData(Guid id)
        {
            var response = await client.GetAsync("auth/UserData?id=" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<UserData> GetUserData(string profileName)
        {
            var response = await client.GetAsync("auth/UserData?profileName=" + profileName);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<bool> UserExists(string profileName)
        {
            var response = await client.GetAsync("auth/UserExists?profileName=" + profileName);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public void ForgotPassword(string email)
        {
            client.GetAsync("auth/ForgotPassword?email=" + email);
        }

        #endregion
    }
}
