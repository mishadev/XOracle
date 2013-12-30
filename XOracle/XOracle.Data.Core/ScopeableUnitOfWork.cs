using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Data.Core
{
    public class ScopeableUnitOfWork : IScopeable<IUnitOfWork>
    {
        private IUnitOfWork _unitOfWork;

        public ScopeableUnitOfWork(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            this._unitOfWork.Commit();
        }

        public async Task Rollback()
        {
            await this._unitOfWork.Rollback();
        }
    }
}
