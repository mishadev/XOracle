using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Azure.Core.Stores.Storage
{
    public interface IListBlobItemWithName : IListBlobItem
    {
        string Name { get; }
    }
}
