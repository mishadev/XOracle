using System;
using System.Collections.Generic;

namespace XOracle.Data.Core
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        void Add(TEntity item);

        void Remove(TEntity item);

        void Modify(TEntity item);

        TEntity Get(Guid id);

        IEnumerable<TEntity> GetAll();
    }
}
