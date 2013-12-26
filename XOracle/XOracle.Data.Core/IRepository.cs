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
        IUnitOfWork UnitOfWork { get; }

        Task Add(TEntity item);

        Task Remove(TEntity item);

        Task Modify(TEntity item);

        Task<TEntity> Get(Guid id);

        Task<IEnumerable<TEntity>> GetFiltered(Expression<Func<TEntity, bool>> filter, int page, int size);
    }
}
