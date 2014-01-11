using Microsoft.WindowsAzure.StorageClient;
using System.Data.Services.Client;

namespace XOracle.Azure.Core.Stores.Storage
{
    public interface IAzureTableRWStrategy
    {
        void ReadEntity(TableServiceContext context, ReadingWritingEntityEventArgs args);
        void WriteEntity(TableServiceContext context, ReadingWritingEntityEventArgs args);
    }
}
