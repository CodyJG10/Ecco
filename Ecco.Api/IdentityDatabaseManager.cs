using Ecco.Entities;
using IdentityModel.Client;
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

        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Username", username),
                new KeyValuePair<string, string>("Password", password)
            });

            var result = await client.PostAsync("api/login", formContent);
            return result;
        }

        public async Task<HttpResponseMessage> Register(string username, string email, string password, string confirmPassword)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Email", email),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("ConfirmPassword", confirmPassword)
            });
            return await client.PostAsync("api/register", formContent);
        }

        public async Task<UserData> GetUserData(Guid id)
        {
            var response = await client.GetAsync("api/UserData?id=" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<UserData> GetUserData(string profileId)
        {
            var response = await client.GetAsync("api/UserData?id=" + profileId);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<UserData> GetUserDataByEmail(string email)
        {
            var response = await client.GetAsync("api/UserData?email=" + email);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public async Task<bool> UserExists(string profileName)
        {
            var response = await client.GetAsync("api/UserExists?profileName=" + profileName);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<bool>(result);
        }

        public void ForgotPassword(string email)
        {
            client.GetAsync("api/ForgotPassword?email=" + email);
        }

        public async Task<HttpResponseMessage> Authenticate(string secret)
        {
            //var disco = await client.GetDiscoveryDocumentAsync(client.BaseAddress.AbsoluteUri);
            var disco = await client.GetDiscoveryDocumentAsync("https://ecco-space.azurewebsites.net");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "ecco-mobile",
                ClientSecret = secret,
                Scope = "api1"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
            }

            client.SetBearerToken(tokenResponse.AccessToken);
            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
