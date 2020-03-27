﻿using Ecco.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public class DatabaseManager : IDatabaseManager
    {
        private readonly HttpClient client;

        public DatabaseManager()
        {
            client = new HttpClient();
        }

        public void SetUrl(string url)
        {
            if (!url.EndsWith("/"))
            {
                url += "/";
            }
            client.BaseAddress = new Uri(url);
        }

        #region Models

        public struct LoginResult
        {
            public string token { get; set; }
        }

        public struct RegisterResult
        { 
            [JsonProperty("status")]
            public int StatusCode { get; set; }
            [JsonProperty("errors")]
            public Error Errors { get; set; }

            public struct Error 
            {
                public List<string> ConfirmPassword { get; set; }
                public List<string> Password { get; set; }
            }
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

        public async Task<RegisterResult> Register(string username, string password, string confirmPassword)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("UserName", username),
                new KeyValuePair<string, string>("Password", password),
                new KeyValuePair<string, string>("ConfirmPassword", confirmPassword)
            });
            var response = await client.PostAsync("auth/register", formContent);
            var result = await response.Content.ReadAsStringAsync();

            var registerResult = JsonConvert.DeserializeObject<RegisterResult>(result);

            return registerResult;
        }

        #endregion

        #region API Access

        public async Task<IEnumerable<Card>> GetCards()
        {
            var response = await client.GetAsync("api/cards");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Card>>(result);
        }

        public async Task<IEnumerable<Connection>> GetConnections(Guid id)
        {
            var response = await client.GetAsync("api/cards?id=" + id);
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Connection>>(result);
        }

        public async Task<UserData> GetUserData()
        {
            var response = await client.GetAsync("api/userinfo");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UserData>(result);
        }

        public struct UserData
        { 
            public Guid Id { get; set; }
            public string UserName { get; set; }
        }
        #endregion
    }
}