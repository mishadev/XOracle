using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XOracle.Domain.Core;

namespace XOracle.Data.Core
{
    public interface IRepository<TEntity> : IRepository, IDisposable
        where TEntity : Entity
    {
        Task Add(TEntity item);

        Task Remove(TEntity item);

        Task Modify(TEntity item);

        Task<TEntity> Get(Guid id);

        Task<TEntity> GetBy(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }

    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
