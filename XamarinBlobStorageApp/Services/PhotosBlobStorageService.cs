using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinBlobStorageApp
{
    public abstract class PhotosBlobStorageService : BaseBlobStorageService
    {
        #region Methods
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            try
            {
                var photoBlob = await SaveBlockBlob(AzureBlobStorageConstants.ContainerName, photo, photoTitle).ConfigureAwait(false);
                return new PhotoModel { Title = photoBlob.Name, Uri = photoBlob.Uri };
            }
            catch (Exception e)
            {
                DebugServices.Log(e);
                return null;
            }
        }

        public static async Task<List<PhotoModel>> GetPhotos()
        {
            var blobList = await GetBlobs<CloudBlockBlob>(AzureBlobStorageConstants.ContainerName).ConfigureAwait(false);

            return blobList.Select(x => new PhotoModel { Title = x.Name, Uri = x.Uri }).ToList();
        }
        #endregion
    }
}
