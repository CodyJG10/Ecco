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
    }
}