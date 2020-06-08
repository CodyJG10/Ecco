using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using Ecco.Entities.Events;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ecco.Web.Services
{
    public class EventsHubService
    {
        private readonly EventHubProducerClient _client;
        private const string EventHubName = "ecco-events";
        private string _eventHubConnectionString;

        public EventsHubService(string connectionString)
        {
            _eventHubConnectionString = connectionString;
        }

        public async void SendEvent(UpdateEvent e)
        {
            if (e == null)
                e = new UpdateEvent()
                {
                    User = null,
                    Message = "No body"
                };

            await using var producerClient = new EventHubProducerClient(_eventHubConnectionString, EventHubName);
            using EventDataBatch eventBatch = await producerClient.CreateBatchAsync();
            eventBatch.TryAdd(new EventData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(e))));
            await producerClient.SendAsync(eventBatch);
        }
    }
}