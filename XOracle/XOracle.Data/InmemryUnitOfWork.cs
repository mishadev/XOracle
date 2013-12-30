using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Data
{
    public partial class InmemoryUnitOfWork : IDictionarySetUnitOfWork
    {
        private static IDictionary<Type, Byte[]> _store = new Dictionary<Type, Byte[]>();
        private IDictionary<Type, object> _local;

        public InmemoryUnitOfWork()
        {
            _local = new Dictionary<Type, object>();
        }

        public async Task Commit()
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            foreach (var key in _local.Keys)
            {
                var store = await this.GetFromStorage(key);
                var local = (IDictionary)_local[key];

                var data = Merge(store, local);

                Replace((IDictionary)_store, key, await serializer.ToBinary(data));
            }
        }

        private IDictionary Merge(IDictionary store, IDictionary local)
        {
            if (store == null)
                return local;

            foreach (var key in local.Keys)
                Replace(store, key, local[key]);

            return store;
        }

        private void Replace(IDictionary store, object key, object value)
        {
            if (store.Contains(key))
                store[key] = value;
            else
                store.Add(key, value);
        }

        public async Task Rollback()
        {
            this.SyncRollback();
        }

        private void SyncRollback()
        {
            foreach (var key in _local.Keys)
            {
                ((IDictionary)_local[key]).Clear();
            }
            _local.Clear();
        }

        public void Dispose()
        {
            this.SyncRollback();
        }

        public async Task<IDictionary<Guid, TEntity>> CreateSet<TEntity>()
            where TEntity : Entity
        {
            var stored = await GetFromStorage<TEntity>();
            var local = UpdateLocal(stored);

            return local;
        }

        private async Task<IDictionary<Guid, TEntity>> GetFromStorage<TEntity>()
        {
            var storage = await this.GetFromStorage(typeof(TEntity));

            return storage as IDictionary<Guid, TEntity> ?? new Dictionary<Guid, TEntity>();
        }

        private async Task<IDictionary> GetFromStorage(Type key)
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            if (_store.ContainsKey(key))
                return (IDictionary)await serializer.FromBinary(_store[key]);

            return null;
        }

        private IDictionary<Guid, TEntity> UpdateLocal<TEntity>(IDictionary<Guid, TEntity> stored)
        {
            var type = typeof(TEntity);

            if (_local.ContainsKey(type))
            {
                var innerlocal = (IDictionary<Guid, TEntity>)_local[type];
                innerlocal.Clear();

                foreach (var item in stored)
                    innerlocal.Add(item);
            }
            else
            {
                _local.Add(type, stored);
            }

            return (IDictionary<Guid, TEntity>)_local[type];
        }
    }
}
