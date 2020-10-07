using Ecco.Api.Util;
using Ecco.Entities;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ecco.Web.Services
{ 
    public class StorageService
    {
        public CloudBlobClient CloudBlobClient { get; set; }

        public StorageService(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient = storageAccount.CreateCloudBlobClient();
        }

        public string GetCardImageData(string username, Card card)
        {
            string containerName = UserToContainer.EmailToContainer(username);
            var container = CloudBlobClient.GetContainerReference(containerName);
            var blob = container.GetBlockBlobReference(card.CardTitle.Replace(" ", "%20") + ".png");
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
            }
            return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
        }

        public string GetTemplateImageData(Template template)
        {
            var container = CloudBlobClient.GetContainerReference("templates");
            var blob = container.GetBlockBlobReference(template.FileName);
            MemoryStream memoryStream;
            using (memoryStream = new MemoryStream())
            {
                blob.DownloadToStream(memoryStream);
            }
            return "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
        }
    }
}