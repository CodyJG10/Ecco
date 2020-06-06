using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;

namespace Ecco.Api.Events
{
    public class CloudEventListener
    {
        public delegate void CloudEventReceived(string message);
        public static event CloudEventReceived OnCloudEventReceived;

        public string EventHubConnectionString { get; set; }
        public string EventHubName { get; set; }
        public string StorageContainerName { get; set; }
        public string StorageConnectionString { get; set;  }

        private EventProcessorClient eventProcessor;

        public async void InitClient()
        {
            string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
            BlobContainerClient storageClient = new BlobContainerClient(StorageConnectionString, StorageContainerName);
            eventProcessor = new EventProcessorClient(storageClient, consumerGroup, EventHubConnectionString, EventHubName);
            eventProcessor.ProcessEventAsync += EventProcessor_ProcessEventAsync; ;
            eventProcessor.ProcessErrorAsync += EventProcessor_ProcessErrorAsync;
            await eventProcessor.StartProcessingAsync();
        }

        private Task EventProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            return Task.CompletedTask;
        }

        private async Task<Task> EventProcessor_ProcessEventAsync(ProcessEventArgs arg)
        {
            await arg.UpdateCheckpointAsync(arg.CancellationToken);
            var data = arg.Data;
            string msg = Encoding.UTF8.GetString(data.Body.ToArray());
            OnCloudEventReceived?.Invoke(msg);
            Console.WriteLine(msg);
            return Task.CompletedTask;
        }
    }
}
