using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
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
    }
}