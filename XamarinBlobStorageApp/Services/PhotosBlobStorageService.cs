using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Blob;

namespace XamarinBlobStorageApp
{
    abstract class PhotosBlobStorageService : BaseBlobStorageService
    {
        public static async Task<PhotoModel> SavePhoto(byte[] photo, string photoTitle)
        {
            var photoBlob = await SaveBlockBlob(AzureBlobStorageConstants.ContainerName, photo, photoTitle).ConfigureAwait(false);
            return new PhotoModel(photoBlob.Uri, photoBlob.Name);
        }

        public static async IAsyncEnumerable<PhotoModel> GetPhotos([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            await foreach (var blob in GetBlobs<CloudBlockBlob>(AzureBlobStorageConstants.ContainerName, cancellationToken).ConfigureAwait(false))
            {
                yield return new PhotoModel(blob.Uri, blob.Name);
            }
        }
    }
}
