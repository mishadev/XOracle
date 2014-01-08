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

        public async Task Rollback()
        {
            foreach (var key in _local.Keys)
            {
                var storage = await GetFromStorage(key);

                this.ResetLocal(key, (IDictionary)storage);
            }
        }

        public void Dispose()
        {
            this.Rollback().GetAwaiter().GetResult();
        }

        public async Task<IDictionary<Guid, TEntity>> CreateSet<TEntity>()
            where TEntity : Entity
        {
            var stored = await this.GetFromStorage<TEntity>();
            var local = this.UpdateLocal(stored);

            return local;
        }

        private async Task<IDictionary<Guid, TEntity>> GetFromStorage<TEntity>()
        {
            var storage = await this.GetFromStorage(typeof(TEntity));

            return storage as IDictionary<Guid, TEntity> ?? new Dictionary<Guid, TEntity>();
        }

        private IDictionary<Guid, TEntity> UpdateLocal<TEntity>(IDictionary<Guid, TEntity> stored)
        {
            return (IDictionary<Guid, TEntity>)this.UpdateLocal(typeof(TEntity), (IDictionary)stored);
        }

        private async Task<IDictionary> GetFromStorage(Type key)
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            if (_store.ContainsKey(key))
                return (IDictionary)await serializer.FromBinary(_store[key]);

            return null;
        }

        private IDictionary UpdateLocal(Type type, IDictionary stored)
        {
            if (this._local.ContainsKey(type))
                this.Merge((IDictionary)this._local[type], stored);
            else
                this._local.Add(type, stored);

            return (IDictionary)this._local[type];
        }

        private IDictionary ResetLocal(Type type, IDictionary stored)
        {
            if (this._local.ContainsKey(type))
                this.Substitution((IDictionary)this._local[type], stored);
            else
                this._local.Add(type, stored);

            return (IDictionary)this._local[type];
        }

        private IDictionary Substitution(IDictionary to, IDictionary from)
        {
            to.Clear();

            return this.Merge(to, from);
        }

        private IDictionary Merge(IDictionary to, IDictionary from)
        {
            if (to == null)
                return from;

            if (from == null)
                return to;

            foreach (var key in from.Keys)
                Replace(to, key, from[key]);

            return to;
        }

        private void Replace(IDictionary store, object key, object value)
        {
            if (store.Contains(key))
                store[key] = value;
            else
                store.Add(key, value);
        }
    }
}
