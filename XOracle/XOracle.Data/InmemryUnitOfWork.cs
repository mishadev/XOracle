using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Data
{
    public class InmemoryUnitOfWork : IDictionarySetUnitOfWork
    {
        private IDictionary<Type, Byte[]> _store;
        private IDictionary<Type, object> _raw;

        public InmemoryUnitOfWork()
        {
            _store = new Dictionary<Type, Byte[]>();
            _raw = new Dictionary<Type, object>();
        }

        public async Task Commit()
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            foreach (var key in _raw.Keys)
            {
                _store.Add(key, await serializer.ToBinary(_raw[key]));
            }
        }

        public async Task Rollback()
        {
            this.SyncRollback();
        }

        private void SyncRollback()
        {
            foreach (var key in _raw.Keys)
            {
                ((IDictionary)_raw[key]).Clear();
            }
            _raw.Clear();
        }

        public void Dispose()
        {
            this.SyncRollback();

            this._store.Clear();
        }

        public async Task<IDictionary<Guid, TEntity>> CreateSet<TEntity>()
            where TEntity : Entity
        {
            var stored = await GetFromStorage<TEntity>();
            var raw = GetFromRaw(stored);

            return raw;
        }

        private async Task<IDictionary<Guid, TEntity>> GetFromStorage<TEntity>()
        {
            var key = typeof(TEntity);
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            if (_store.ContainsKey(key))
                return (IDictionary<Guid, TEntity>)await serializer.FromBinary(_store[key]);

            return new Dictionary<Guid, TEntity>();
        }

        private IDictionary<Guid, TEntity> GetFromRaw<TEntity>(IDictionary<Guid, TEntity> stored)
        {
            var type = typeof(TEntity);

            if (_raw.ContainsKey(type))
            {
                var innerRaw = (IDictionary<Guid, TEntity>)_raw[type];
                innerRaw.Clear();

                foreach (var item in stored)
                    innerRaw.Add(item);
            }
            else
            {
                _raw.Add(type, stored);
            }

            return (IDictionary<Guid, TEntity>)_raw[type];
        }
    }
}
