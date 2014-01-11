using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XOracle.Domain.Core;

namespace XOracle.Data.Core
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : Entity
    {
        Task Add(TEntity item);

        Task Add(IEnumerable<TEntity> items);

        Task Remove(TEntity item);

        Task Remove(IEnumerable<TEntity> items);

        Task Modify(TEntity item);

        Task Modify(IEnumerable<TEntity> items);

        Task<TEntity> Get(Guid id);

        Task<TEntity> GetBy(Expression<Func<TEntity, bool>> filter);

        Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }
}
