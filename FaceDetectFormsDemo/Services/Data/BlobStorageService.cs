using System.Threading.Tasks;
using FaceDetectFormsDemo.Constants;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace FaceDetectFormsDemo.Services
{
    public class BlobStorageService: IBlobStorageService
    {
        readonly static CloudStorageAccount _cloudStorageAccount = CloudStorageAccount.Parse(BlobStorageConstants.BLOB_STORAGE_ENDPOINT);
        readonly static CloudBlobClient _blobClient = _cloudStorageAccount.CreateCloudBlobClient();

        public async Task<string> SaveBlockBlob(byte[] blob, string blobTitle)
        {
            var blobContainer = _blobClient.GetContainerReference(BlobStorageConstants.BLOB_STORAGE_CONTAINER_NAME);

            var blockBlob = blobContainer.GetBlockBlobReference(blobTitle);
            blockBlob.Properties.ContentType = "image/png";
            await blockBlob.UploadFromByteArrayAsync(blob, 0, blob.Length);

            return blockBlob?.Uri?.AbsoluteUri;
        }
    }
}
