using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Runtime.CompilerServices;
using System.Threading;

namespace XamarinBlobStorageApp
{
    public abstract class BaseBlobStorageService
    {
        readonly static Lazy<CloudStorageAccount> _cloudStorageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(AzureBlobStorageConstants.ConnectionString));
        readonly static Lazy<CloudBlobClient> _blobClientHolder = new Lazy<CloudBlobClient>(_cloudStorageAccountHolder.Value.CreateCloudBlobClient);

        static CloudBlobClient BlobClient => _blobClientHolder.Value;

        protected static async Task<CloudBlockBlob> SaveBlockBlob(string containerName, byte[] blob, string blobTitle)
        {
            var blobContainer = GetBlobContainer(containerName);

            var blockBlob = blobContainer.GetBlockBlobReference(blobTitle);
            await blockBlob.UploadFromByteArrayAsync(blob, 0, blob.Length).ConfigureAwait(false);

            return blockBlob;
        }

        protected static async IAsyncEnumerable<T> GetBlobs<T>(string containerName, [EnumeratorCancellation] CancellationToken cancellationToken, string prefix = "", int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) where T : ICloudBlob
        {
            var blobContainer = GetBlobContainer(containerName);
            BlobContinuationToken? continuationToken = null;

            do
            {
                var response = await blobContainer.ListBlobsSegmentedAsync(prefix, true, blobListingDetails, maxresultsPerQuery, continuationToken, null, null, cancellationToken);
                continuationToken = response.ContinuationToken;

                foreach (var blob in response.Results.OfType<T>())
                {
                    yield return blob;
                }

            } while (continuationToken != null);
        }

        static CloudBlobContainer GetBlobContainer(string containerName) => BlobClient.GetContainerReference(containerName);
    }
}
