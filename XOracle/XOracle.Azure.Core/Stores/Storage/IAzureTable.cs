using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XOracle.Azure.Core.Stores.Storage
{
    public interface IAzureTable<T> : IAzureObjectWithRetryPolicyFactory where T : TableServiceEntity
    {
        IQueryable<T> Query { get; }
        CloudStorageAccount Account { get; }

        Task EnsureExist();
        Task Add(T entity);
        Task Add(IEnumerable<T> entities);
        Task AddOrUpdate(T entity);
        Task AddOrUpdate(IEnumerable<T> entities);
        Task Delete(T entity);
        Task Delete(IEnumerable<T> entities);
    }
}
