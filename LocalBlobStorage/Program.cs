using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace LocalBlobStorage
{
    internal class Program
    {
        public static CloudStorageAccount StorageAccount { get; set; }
        public static CloudBlobContainer Container { get; set; }

        private static void Main()
        {
            StorageAccount = CloudStorageAccount.DevelopmentStorageAccount;

            CreateContainerAsync().Wait();
            InsertDocumentAsync().Wait();
            DeleteContainerAsync().Wait();
        }

        private static async Task CreateContainerAsync()
        {
            var blobClient = StorageAccount.CreateCloudBlobClient();
            var containerName = Guid.NewGuid().ToString();
            Container = blobClient.GetContainerReference(containerName);
            await Container.CreateAsync();
        }

        private static async Task InsertDocumentAsync()
        {
            var trackingId = Guid.NewGuid().ToString();
            var message = new BusMessage
            {
                TrackingId = trackingId
            };

            var messageText = JsonConvert.SerializeObject(message);

            var blockBlob = Container.GetBlockBlobReference(trackingId);            
            await blockBlob.UploadTextAsync(messageText);
            Console.WriteLine(blockBlob.Uri);
            Console.ReadLine();
        }

        private static async Task DeleteContainerAsync()
        {
            await Container.DeleteAsync();
        }
    }
}
