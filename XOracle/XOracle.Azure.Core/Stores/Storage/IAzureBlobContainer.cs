using System;
using System.Collections.Generic;

namespace XOracle.Azure.Core.Stores.Storage
{
    public interface IAzureBlobContainer<T> : IAzureObjectWithRetryPolicyFactory
    {
        void EnsureExist();

        bool AcquireLock(PessimisticConcurrencyContext context);
        void ReleaseLock(PessimisticConcurrencyContext context);

        T Get(string objId);
        T Get(string objId, out OptimisticConcurrencyContext context);
        IEnumerable<IListBlobItemWithName> GetBlobList();
        Uri GetUri(string objId);

        void Delete(string objId);
        void DeleteContainer();

        void Save(string objId, T entity);
        void Save(IConcurrencyControlContext context, T entity);
    }
}
