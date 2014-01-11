using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Azure.Core.Helpers;
using XOracle.Infrastructure.Core;

namespace XOracle.Azure.Core.Stores.Storage
{
    public class AzureTable<T> : AzureStorageWithRetryPolicy, IAzureTable<T>
        where T : TableServiceEntity
    {
        private readonly string _tableName;
        private readonly CloudStorageAccount _account;
        private readonly ILogger _logger;

        public AzureTable(CloudStorageAccount account)
            : this(account, typeof(T).Name)
        {
        }

        public AzureTable(CloudStorageAccount account, string tableName)
        {
            this._tableName = tableName;
            this._account = account;
            this._logger = Factory<ILogger>.GetInstance();
        }

        public CloudStorageAccount Account
        {
            get
            {
                return this._account;
            }
        }

        public IQueryable<T> Query
        {
            get
            {
                TableServiceContext context = this.CreateContext();
                return context.CreateQuery<T>(this._tableName).AsTableServiceQuery();
            }
        }

        public IAzureTableRWStrategy ReadWriteStrategy { get; set; }

        public async Task EnsureExist()
        {
            var cloudTableClient = new CloudTableClient(this._account.TableEndpoint.ToString(), this._account.Credentials);

            await this.StorageRetryPolicy.ExecuteAsync(this.CreateTask(
                async => cloudTableClient.BeginCreateTableIfNotExist(this._tableName, async, null),
                res => cloudTableClient.EndCreateTableIfNotExist(res)));
        }

        public async Task Add(T entity)
        {
            await this.Add(new[] { entity });
        }

        public async Task Add(IEnumerable<T> entities)
        {
            TableServiceContext context = this.CreateContext();

            foreach (var entity in entities)
                context.AddObject(this._tableName, entity);

            var saveChangesOptions = SaveChangesOptions.None;
            if (entities.Distinct(new PartitionKeyComparer()).Count() == 1)
                saveChangesOptions = SaveChangesOptions.Batch;

            await this.SaveChangesAsync(context, saveChangesOptions);
        }

        public async Task AddOrUpdate(T entity)
        {
            await this.AddOrUpdate(new[] { entity });
        }

        public async Task AddOrUpdate(IEnumerable<T> entities)
        {
            foreach (T entity in entities)
            {
                var success = await this.StorageRetryPolicy.ExecuteAsync<bool>(() => this.TryAdd(entity));

                if (!success) await Update(entity);
            }
        }

        private async Task Update(T entity)
        {
            TableServiceContext context = this.CreateContext();
            context.AttachTo(this._tableName, entity, "*");
            context.UpdateObject(entity);

            await this.SaveChangesAsync(context, SaveChangesOptions.ReplaceOnUpdate);
        }

        private async Task<bool> TryAdd(T entity)
        {
            var can = false;
            var existing = default(T);

            try
            {
                existing = this.Query.Where(o => o.PartitionKey == entity.PartitionKey && o.RowKey == entity.RowKey).SingleOrDefault();
            }
            catch (DataServiceQueryException ex)
            {
                var dataServiceClientException = ex.InnerException as DataServiceClientException;
                if (dataServiceClientException != null)
                {
                    if (dataServiceClientException.StatusCode == 404)
                    {
                        this._logger.LogWarning(ex.TraceInformation());

                        can = true;
                    }
                    else
                    {
                        this._logger.LogError(ex.TraceInformation());
                        throw;
                    }
                }
                else
                {
                    this._logger.LogError(ex.TraceInformation());
                    throw;
                }
            }

            bool added;
            if (added = can || existing == default(T))
                await this.Add(entity);

            return added;
        }

        public async Task Delete(T entity)
        {
            await this.Delete(new[] { entity });
        }

        public async Task Delete(IEnumerable<T> entities)
        {
            TableServiceContext context = this.CreateContext();
            foreach (var entity in entities)
            {
                context.AttachTo(this._tableName, entity, "*");
                context.DeleteObject(entity);
            }

            await this.SaveChangesAsync(context, context.SaveChangesDefaultOptions);
        }

        private TableServiceContext CreateContext()
        {
            var context = new TableServiceContext(this._account.TableEndpoint.ToString(), this._account.Credentials)
            {
                // retry policy is handled by TFHAB
                RetryPolicy = RetryPolicies.NoRetry()
            };

            if (this.ReadWriteStrategy != null)
            {
                context.ReadingEntity += (sender, args) => this.ReadWriteStrategy.ReadEntity(context, args);
                context.WritingEntity += (sender, args) => this.ReadWriteStrategy.WriteEntity(context, args);
            }

            return context;
        }

        private class PartitionKeyComparer : IEqualityComparer<TableServiceEntity>
        {
            public bool Equals(TableServiceEntity x, TableServiceEntity y)
            {
                return string.Compare(x.PartitionKey, y.PartitionKey, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
            }

            public int GetHashCode(TableServiceEntity entity)
            {
                return entity.PartitionKey.GetHashCode();
            }
        }

        private async Task SaveChangesAsync(TableServiceContext context, SaveChangesOptions options)
        {
            await this.StorageRetryPolicy.ExecuteAsync(this.CreateTask(
                async => context.BeginSaveChanges(options, async, null),
                res => context.EndSaveChanges(res)));
        }

        private Func<Task> CreateTask(Func<AsyncCallback, IAsyncResult> begin, Action<IAsyncResult> end)
        {
            return () => Task.Factory.FromAsync((asyn, state) => begin(asyn), end, null);
        }
    }
}
