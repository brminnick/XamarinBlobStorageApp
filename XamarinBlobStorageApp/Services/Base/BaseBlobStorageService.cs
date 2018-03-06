using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinBlobStorageApp
{
    public abstract class BaseBlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<CloudStorageAccount> _cloudStorageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(AzureBlobStorageConstants.ConnectionString));
        readonly static Lazy<CloudBlobClient> _blobClientHolder = new Lazy<CloudBlobClient>(_cloudStorageAccountHolder.Value.CreateCloudBlobClient);
        readonly static Lazy<CloudBlobContainer> _blobContainerHolder = new Lazy<CloudBlobContainer>(() => _blobClientHolder.Value.GetContainerReference(AzureBlobStorageConstants.ContainerName));
        #endregion

        #region Properties
        static CloudBlobContainer BlobContainer => _blobContainerHolder.Value;
        #endregion

        #region Methods
        protected static async Task<CloudBlockBlob> SaveBlockBlob(byte[] blob, string blobTitle)
        {
            var blockBlob = BlobContainer.GetBlockBlobReference(blobTitle);
            await blockBlob.UploadFromByteArrayAsync(blob, 0, blob.Length).ConfigureAwait(false);

            return blockBlob;
        }

        protected static async Task<List<T>> GetBlobs<T>(string prefix = "", int? maxresultsPerQuery = null, BlobListingDetails blobListingDetails = BlobListingDetails.None) where T : ICloudBlob
        {
            var blobList = new List<T>();
            BlobContinuationToken continuationToken = null;

            try
            {
                do
                {
                    var response = await BlobContainer.ListBlobsSegmentedAsync(prefix,
                                                                               true,
                                                                               blobListingDetails,
                                                                               maxresultsPerQuery,
                                                                               continuationToken,
                                                                               null,
                                                                               null).ConfigureAwait(false);
                    continuationToken = response?.ContinuationToken;

                    foreach (var blob in response?.Results?.OfType<T>())
                    {
                        blobList.Add(blob);
                    }

                } while (continuationToken != null);
            }
            catch (Exception e)
            {
                DebugServices.Log(e);
            }

            return blobList;
        }
        #endregion
    }
}
