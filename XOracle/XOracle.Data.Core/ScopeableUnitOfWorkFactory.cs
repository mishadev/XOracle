using System.Threading;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Data.Core
{
    public class ScopeableUnitOfWorkFactory : IFactory<IScopeable<IUnitOfWork>>
    {
        private IUnitOfWork _unitOfWork;

        public ScopeableUnitOfWorkFactory(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<IScopeable<IUnitOfWork>> Create()
        {
            return new ScopeableUnitOfWork(this._unitOfWork);
        }
    }
}
