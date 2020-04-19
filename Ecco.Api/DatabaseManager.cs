using System;
using System.Net.Http;

namespace Ecco.Api
{
    public partial class DatabaseManager : IDatabaseManager
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
    }
}