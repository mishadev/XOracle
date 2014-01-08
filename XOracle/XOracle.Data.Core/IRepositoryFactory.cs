using XOracle.Domain.Core;

namespace XOracle.Data.Core
{
    public interface IRepositoryFactory
    {
        IRepository<TEntity> Get<TEntity>()
            where TEntity : Entity;
    }
}
