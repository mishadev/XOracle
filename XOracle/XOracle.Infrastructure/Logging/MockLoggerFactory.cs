using System.Threading.Tasks;

using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Core.Logging;

namespace XOracle.Infrastructure.Logging
{
    public class MockLoggerFactory : ILoggerFactory
    {
        public async Task<ILogger> Create()
        {
            return new MockLogger();
        }
    }
}
