using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Web.Services
{
    public class EventsHubService
    {
        private readonly EventHubProducerClient _client;
        private const string EventHubName = "ecco-events";

        public EventsHubService(string connectionString)
        { 
            _client = new EventHubProducerClient(connectionString, EventHubName);
        }

        public async void SendEvent(string message)
        {
            if (message == null)
                message = "Test";
            using EventDataBatch eventBatch = await _client.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(message)));
            await _client.SendAsync(eventBatch);
        }
    }
}