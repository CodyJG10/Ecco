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

        #endregion

        #region Cards

        public async Task<IEnumerable<Card>> GetCards()
        {
            var response = await client.GetAsync("api/cards");
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Card>>(result);
        }

        public async Task<bool> CreateCard(Card card)
        {
            string cardJson = JsonConvert.SerializeObject(card);
            var content = new StringContent(cardJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/CreateCard", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<Card> GetCard(int cardId)
        {
            var response = await client.GetAsync("api/card?id=" + cardId);
            var result = await response.Content.ReadAsStringAsync();
            var card = JsonConvert.DeserializeObject<Card>(result);
            return card;
        }

        public async Task<IEnumerable<Card>> GetMyCards(string id)
        {
            var response = await client.GetAsync("api/mycards?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Card>>(content);
        }

        public async Task<bool> EditCard(Card card)
        {
            string cardJson = JsonConvert.SerializeObject(card);
            var content = new StringContent(cardJson, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("api/EditCard", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCard(Card card)
        {
            int id = card.Id;
            var response = await client.DeleteAsync("api/DeleteCard?id=" + id);
            return response.IsSuccessStatusCode;
        }

        #endregion

        #region Connections
        
        public async Task<IEnumerable<Connection>> GetMyConnections(Guid id)
        {
            var response = await client.GetAsync("api/MyConnections?id=" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Connection>>(result);
        }

        public async Task<IEnumerable<Connection>> GetMyPendingConnections(Guid id)
        {
            var response = await client.GetAsync("api/MyPendingConnections?id=" + id.ToString());
            var result = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<Connection>>(result);
        }

        public async Task<bool> CreateConnection(Guid id, Guid toId, int cardId)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", id.ToString()),
                new KeyValuePair<string, string>("toId", toId.ToString()),
                new KeyValuePair<string, string>("cardId", cardId.ToString())
            });

            var response = await client.PostAsync("api/CreateConnection", formContent);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AcceptConnection(Connection connection)
        {
            string connectionJson = JsonConvert.SerializeObject(connection);

            var content = new StringContent(connectionJson, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/AcceptConnection", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteConnection(Connection connection)
        {
            string connectionJson = JsonConvert.SerializeObject(connection);
            var content = new StringContent(connectionJson, Encoding.UTF8, "application/json");

            var response = await client.PutAsync("api/DeleteConnection", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<Connection> GetConnection(int id)
        {
            var response = await client.GetAsync("api/connection?id=" + id);
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Connection>(content);
        }
        #endregion

        #region Templates

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

        #endregion
    }
}