using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Azure.Core.Stores.Storage
{
    internal class AzureBlob : CloudBlob, IListBlobItemWithName
    {
        internal AzureBlob(CloudBlob source) : base(source) { }
    }
}
