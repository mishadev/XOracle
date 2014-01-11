using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;

namespace XOracle.Azure.Core.Stores.Storage
{
    public class EntitiesBlobContainer<T> : AzureBlobContainer<T>
    {
        public EntitiesBlobContainer(CloudStorageAccount account)
            : base(account)
        {
        }

        public EntitiesBlobContainer(CloudStorageAccount account, string containerName)
            : base(account, containerName)
        {
        }

        protected override T ReadObject(CloudBlob blob)
        {
            return new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Deserialize<T>(blob.DownloadText());
        }

        protected override byte[] BinarizeObjectForStreaming(BlobProperties properties, T entity)
        {
            properties.ContentType = "application/json";
            var stream = new MemoryStream();
            using (var writer = new StreamWriter(stream, Encoding.Default))
            {
                writer.Write(new JavaScriptSerializer().Serialize(entity));
                writer.Flush();

                return stream.ToArray();
            }
        }

        protected override void WriteObject(CloudBlob blob, BlobRequestOptions options, T entity)
        {
            blob.Properties.ContentType = "application/json";
            blob.UploadText(new JavaScriptSerializer() { MaxJsonLength = int.MaxValue }.Serialize(entity), Encoding.Default, options);
        }
    }
}
