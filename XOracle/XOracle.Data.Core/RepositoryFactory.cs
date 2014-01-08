using XOracle.Domain.Core;

namespace XOracle.Data.Core
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private IUnitOfWork _unitOfWork;

        public RepositoryFactory(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IRepository<TEntity> Get<TEntity>()
            where TEntity : Entity
        {
            return new Repository<TEntity>(this._unitOfWork);
        }
    }
}
