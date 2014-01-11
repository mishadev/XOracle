using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Azure.Core.Stores.Storage
{
    public class FilesBlobContainer : AzureBlobContainer<byte[]>
    {
        private readonly string contentType;

        public FilesBlobContainer(CloudStorageAccount account, string containerName, string contentType)
            : base(account, containerName)
        {
            this.contentType = contentType;
        }

        public override void EnsureExist()
        {
            this.StorageRetryPolicy.ExecuteAction(() =>
            {
                this.Container.CreateIfNotExist();
                this.Container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            });
        }

        protected override byte[] ReadObject(CloudBlob blob)
        {
            return blob.DownloadByteArray();
        }

        protected override void WriteObject(CloudBlob blob, BlobRequestOptions options, byte[] obj)
        {
            blob.Properties.ContentType = this.contentType;
            blob.UploadByteArray(obj, options);
        }

        protected override byte[] BinarizeObjectForStreaming(BlobProperties properties, byte[] obj)
        {
            properties.ContentType = this.contentType;
            return obj;
        }
    }
}
