using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XOracle.Azure.Core.Stores.Storage;
using XOracle.Data.Core;
using XOracle.Domain.Core;

namespace XOracle.Data.Azure
{
    public class AzureRepository<TAzureEntity, TEntity> : IRepository<TEntity>
        where TAzureEntity : TableServiceEntity
        where TEntity : Entity
    {
        private readonly IAzureTable<TAzureEntity> _table;

        public AzureRepository(CloudStorageAccount storageAccount)
        {
            this._table = typeof(IEnum).IsAssignableFrom(typeof(TEntity)) ?
                new AzureTable<TAzureEntity>(storageAccount, "Enum") :
                new AzureTable<TAzureEntity>(storageAccount, typeof(TEntity).Name);
            
            this._table.EnsureExist().GetAwaiter().GetResult();
        }

        public async Task Add(TEntity item)
        {
            await this.Add(new[] { item });
        }

        public async Task Add(IEnumerable<TEntity> items)
        {
            var azureItems = items.Select(Convert);

            await this._table.Add(azureItems);
        }

        public async Task Remove(TEntity item)
        {
            await this.Remove(new[] { item });
        }

        public async Task Remove(IEnumerable<TEntity> items)
        {
            var azureItems = items.Select(Convert);

            await this._table.Delete(azureItems);
        }

        public async Task Modify(TEntity item)
        {
            await this.Modify(new[] { item });
        }

        public async Task Modify(IEnumerable<TEntity> items)
        {
            var azureItems = items.Select(Convert);

            await this._table.AddOrUpdate(azureItems);
        }

        public Task<TEntity> Get(Guid id)
        {
            var query = this._table.Query.Where(row => row.RowKey == id.ToString());

            var azureItem = this._table
                .GetRetryPolicyFactoryInstance()
                .GetDefaultAzureStorageRetryPolicy()
                .ExecuteAction<TAzureEntity>(() => query.SingleOrDefault());

            return Task.FromResult(Convert(azureItem));
        }

        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> filter)
        {
            return (await this.GetFiltered(filter)).SingleOrDefault();
        }

        public Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(this._table.Query.Where(Convert(filter)).Select(Convert));
        }

        private Expression<Func<TAzureEntity, bool>> Convert(Expression<Func<TEntity, bool>> filter)
        {
            return AzureEntityFactory.ToAzureFilter<TAzureEntity, TEntity>(filter);
        }

        private TAzureEntity Convert(TEntity item)
        {
            return AzureEntityFactory.ToAzureEntity<TAzureEntity, TEntity>(item);
        }

        private TEntity Convert(TAzureEntity item)
        {
            return AzureEntityFactory.FromAzureEntity<TAzureEntity, TEntity>(item);
        }

        public void Dispose() { }
    }
}
