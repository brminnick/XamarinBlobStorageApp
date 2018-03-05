using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinBlobStorageApp
{
    public static class BlobStorageService
    {
        #region Constant Fields
        readonly static Lazy<CloudStorageAccount> _cloudStorageAccountHolder = new Lazy<CloudStorageAccount>(() => CloudStorageAccount.Parse(@"DefaultEndpointsProtocol=https;AccountName=blobstoragesampleapp;AccountKey=vBIsmZhnBEoDtkqMLoY+znfG3h+Vg1bByZtOBl35FNCFvZwqDi9oBT22yNpNviK4cT45aP+67133/P8uqoZ8lA==;EndpointSuffix=core.windows.net"));
        readonly static Lazy<CloudBlobClient> _blobClientHolder = new Lazy<CloudBlobClient>(()=> _cloudStorageAccountHolder.Value.CreateCloudBlobClient());
        readonly static Lazy<CloudBlobContainer> _blobContainerHolder = new Lazy<CloudBlobContainer>(() => _blobClientHolder.Value.GetContainerReference(AzureBlobStorageConstants.ContainerName));
        #endregion

        #region Properties
        static CloudBlobContainer BlobContainer => _blobContainerHolder.Value;
        #endregion

        #region Methods
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            var blockBlob = BlobContainer.GetBlockBlobReference(photoTitle);
            await blockBlob.UploadFromByteArrayAsync(photo, 0, photo.Length).ConfigureAwait(false);

            return new PhotoModel { Title = photoTitle, Uri = blockBlob.Uri };
        }

        public static async Task<List<PhotoModel>> GetPhotos()
        {
            var uriList = new List<PhotoModel>();
            BlobContinuationToken continuationToken = null;

            try
            {
                do
                {
                    var response = await BlobContainer.ListBlobsSegmentedAsync(string.Empty,
                                                                               true,
                                                                               BlobListingDetails.None,
                                                                               new int?(),
                                                                               continuationToken,
                                                                               null,
                                                                               null).ConfigureAwait(false);
                    continuationToken = response.ContinuationToken;

                    foreach (var blob in response.Results.OfType<CloudBlockBlob>())
                    {
                        var photo = new PhotoModel { Uri = blob.Uri, Title = blob.Name };
                        uriList.Add(photo);
                    }

                } while (continuationToken != null);
            }
            catch(Exception e)
            {
                DebugServices.Log(e); 
            }

            return uriList;
        }
        #endregion
    }
}
