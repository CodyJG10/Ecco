using Ecco.Entities;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Ecco.Api
{
    public class StorageManager : IStorageManager
    {
        public CloudBlobClient CloudBlobClient { get; set; }

        public StorageManager(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public MemoryStream GetTemplate(string file)
        {
            var container = CloudBlobClient.GetContainerReference("templates");
            var blob = container.GetBlockBlobReference(file);
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
            }
            return memoryStream;
        }

        public MemoryStream GetCard(string username, string file)
        {
            string containerName = "username-" + username.Replace("@", "-").Replace(".", "-");
            var container = CloudBlobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(file.Replace(" ", "%20") + ".png");
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
            }
            return memoryStream;
        }

        public async Task<Task> SaveCard(string cardTitle, Stream file, string username)
        {
            var container = CloudBlobClient.GetContainerReference("username-" + username.Replace("@", "-").Replace(".", "-"));
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlockBlobReference(cardTitle.Replace(" ", "%20") + ".png");
            await blob.UploadFromStreamAsync(file);
            
            return Task.CompletedTask;
        }
    }
}