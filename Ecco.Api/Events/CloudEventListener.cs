using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using Azure.Storage.Blobs;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using Azure.Messaging.EventHubs.Processor;
using System.Diagnostics.Contracts;

namespace Ecco.Api.Events
{
    //public class CloudEventListener
    //{
    //    public delegate void CloudEventReceived(string message);
    //    public static event CloudEventReceived OnCloudEventReceived;

    //    private readonly string eventHubConnectionString;
    //    private readonly string eventHubName;
    //    private readonly string storageContainerName;
    //    private readonly string storageConnectionString;

    //    private EventProcessorClient eventProcessor;

    //    public CloudEventListener(string eventHubConnectionString,
    //                              string eventHubName,
    //                              string storageContainerName,
    //                              string storageConnectionString)
    //    {
    //        this.eventHubConnectionString = eventHubConnectionString;
    //        this.eventHubName = eventHubName;
    //        this.storageConnectionString = storageConnectionString;
    //        this.storageContainerName = storageContainerName;
    //        InitClient();
    //    }

    //    public async void InitClient()
    //    {
    //        string consumerGroup = EventHubConsumerClient.DefaultConsumerGroupName;
    //        BlobContainerClient storageClient = new BlobContainerClient(storageConnectionString, storageContainerName);
    //        eventProcessor = new EventProcessorClient(storageClient, consumerGroup, eventHubConnectionString, eventHubName);

    //        eventProcessor.ProcessEventAsync += EventProcessor_ProcessEventAsync; ;
    //        eventProcessor.ProcessErrorAsync += EventProcessor_ProcessErrorAsync;

    //        try
    //        {
    //            await eventProcessor.StartProcessingAsync();
    //        }
    //        catch (Exception e)
    //        {
    //            Console.WriteLine(e.Message);
    //        }
    //    }

    //    private Task EventProcessor_ProcessErrorAsync(ProcessErrorEventArgs arg)
    //    {
    //        return Task.CompletedTask;
    //    }

    //    private async Task<Task> EventProcessor_ProcessEventAsync(ProcessEventArgs arg)
    //    {
    //        var data = arg.Data;
    //        string msg = Encoding.UTF8.GetString(data.Body.ToArray());
    //        OnCloudEventReceived?.Invoke(msg);
    //        await arg.UpdateCheckpointAsync(arg.CancellationToken);
    //        Console.WriteLine(msg);
    //        return Task.CompletedTask;
    //    }

    //    public async void Stop()
    //    {
    //        await eventProcessor.StopProcessingAsync();
    //    }

    //    public async void Start()
    //    {
    //        await eventProcessor.StartProcessingAsync();
    //    }
    //}
}
