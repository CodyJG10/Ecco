using System;
using System.Collections.Generic;
using System.Text;
using Ecco.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public partial class DatabaseManager
    {
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

        public async Task<Card> GetActiveCard(string userId)
        {
            var response = await client.GetAsync("api/ActiveCard?userId=" + userId);
            var content = await response.Content.ReadAsStringAsync();
            if (content == null)
                return null;
            return await GetCard(int.Parse(content));
        }

        public async Task<HttpResponseMessage> UpdateActiveCard(string userId, string cardId)
        {
            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("userId", userId),
                new KeyValuePair<string, string>("cardId", cardId)
            });

            var response = await client.PostAsync("api/UpdateActiveCard", formContent);
            return response;
        }
    }
}