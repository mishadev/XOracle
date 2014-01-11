using XOracle.Data.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Data
{
    public class AzureScopeableUnitOfWorkFactory : IFactory<IScopeable<IUnitOfWork>>
    {
        public IScopeable<IUnitOfWork> Create()
        {
            return new AzureScopeableUnitOfWork();
        }
    }
}
