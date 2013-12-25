using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XOracle.Data.Core
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        IUnitOfWork UnitOfWork { get; }

        Task Add(TEntity item);

        Task Remove(TEntity item);

        Task Modify(TEntity item);

        Task<TEntity> Get(Guid id);

        Task<IEnumerable<TEntity>> GetAll();
    }
}
