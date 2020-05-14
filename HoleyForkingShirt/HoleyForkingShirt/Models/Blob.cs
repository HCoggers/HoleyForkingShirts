using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HoleyForkingShirt.Models
{
    public class Blob
    {
        public CloudStorageAccount CloudStorageAccount { get; set; }
        public CloudBlobClient CloudBlobClient { get; set; }

        public Blob(IConfiguration configuration)
        {
            var storageCreds = new StorageCredentials(configuration["Storage-Account-Name"], configuration["BlobKey"]);
            CloudStorageAccount = new CloudStorageAccount(storageCreds, true);
            CloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
        }

        /// <summary>
        /// Gets our container from the Azure Blob
        /// </summary>
        /// <param name="containerName">name of container to search for</param>
        /// <returns>the found container</returns>
        public async Task<CloudBlobContainer> GetContainerAsync(string containerName)
        {
            CloudBlobContainer container = CloudBlobClient.GetContainerReference(containerName);

            await container.CreateIfNotExistsAsync();
            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return container;
        }

        /// <summary>
        /// Gets a specified blob from a specified container;
        /// </summary>
        /// <param name="blobName">name of blob to find</param>
        /// <param name="containerName">name of container to search</param>
        /// <returns>found blob</returns>
        public async Task<CloudBlob> GetBlobAsync(string blobName, string containerName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlob blob = container.GetBlobReference(blobName);

            return blob;
        }

        /// <summary>
        /// Uploads a file to a specified container, from a path
        /// </summary>
        /// <param name="containerName">container to upload to</param>
        /// <param name="fileName">name of file to be uploaded</param>
        /// <param name="path">local path of file</param>
        /// <returns>No Content</returns>
        public async Task UploadFileAsync(string containerName, string fileName, string path)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlockBlob block = container.GetBlockBlobReference(fileName);

            await block.UploadFromFileAsync(path);
        }

        /// <summary>
        /// Deletes a blob from a specified container
        /// </summary>
        /// <param name="containerName">container holding blob to be deleted</param>
        /// <param name="blobName">name of blob to be deleted</param>
        /// <returns>No Content</returns>
        public async Task DeleteBlobAsync(string containerName, string blobName)
        {
            CloudBlobContainer container = await GetContainerAsync(containerName);
            CloudBlockBlob block = container.GetBlockBlobReference(blobName);

            await block.DeleteAsync();
        }
    }
}
