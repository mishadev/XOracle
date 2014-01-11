using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Data
{
    public class AzureScopeableUnitOfWork : IScopeable<IUnitOfWork>
    {
        public async Task Rollback() { }

        public void Dispose() { }
    }
}
