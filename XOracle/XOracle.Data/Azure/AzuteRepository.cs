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
using XOracle.Infrastructure.Core;

namespace XOracle.Data.Azure
{
    public class AzureRepository<TAzureEntity, TEntity> : IRepository<TEntity>
        where TAzureEntity : TableServiceEntity
        where TEntity : Entity
    {
        private readonly static IValidator _validator = Factory<IValidator>.GetInstance();

        private readonly IAzureTable<TAzureEntity> _table;
        private bool _inited = false;

        public AzureRepository(CloudStorageAccount storageAccount)
        {
            this._table = typeof(IEnum).IsAssignableFrom(typeof(TEntity)) ?
                new AzureTable<TAzureEntity>(storageAccount, "Enum") :
                new AzureTable<TAzureEntity>(storageAccount, typeof(TEntity).Name);
        }

        public async Task EnsureInitialize()
        {
            if (!this._inited)
            {
                await this._table.EnsureExist();

                this._inited = true;
            }
        }

        public async Task Add(TEntity item)
        {
            await this.Add(new[] { item });
        }

        public async Task Add(IEnumerable<TEntity> items)
        {
            await this.EnsureInitialize();

            var azureItems = items.Select(i => {
                Validate(i);
                i.EnsureIdentity();
                return Convert(i);
            });

            await this._table.Add(azureItems);
        }

        public async Task Remove(TEntity item)
        {
            await this.Remove(new[] { item });
        }

        public async Task Remove(IEnumerable<TEntity> items)
        {
            await this.EnsureInitialize();

            var azureItems = items.Select(Convert);

            await this._table.Delete(azureItems);
        }

        public async Task Modify(TEntity item)
        {
            await this.Modify(new[] { item });
        }

        public async Task Modify(IEnumerable<TEntity> items)
        {
            await this.EnsureInitialize();

            var azureItems = items.Select(Convert);

            await this._table.AddOrUpdate(azureItems);
        }

        public async Task<TEntity> Get(Guid id)
        {
            return await this.GetBy(e => e.Id == id);
        }

        public async Task<TEntity> GetBy(Expression<Func<TEntity, bool>> filter)
        {
            return (await this.GetFiltered(filter)).SingleOrDefault();
        }

        public async Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            await this.EnsureInitialize();

            try
            {
                var x = Convert(filter);
                var b = x.Compile();

                return this._table.Query.Where(x).Select(Convert);
            }
            catch (Exception)
            {
                
                throw;
            }
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

        private static void Validate(TEntity item)
        {
            var validationErrors = _validator.GetErrorMessages(item);

            if (validationErrors.Any())
                throw new InvalidOperationException("validation errors: " + string.Join(", ", validationErrors));
        }
    }
}
