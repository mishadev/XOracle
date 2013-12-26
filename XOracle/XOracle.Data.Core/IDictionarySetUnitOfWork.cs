using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XOracle.Domain.Core;

namespace XOracle.Data.Core
{
    public interface IDictionarySetUnitOfWork : IUnitOfWork
    {
        Task<IDictionary<Guid, TEntity>> CreateSet<TEntity>() where TEntity : Entity;
    }
}
