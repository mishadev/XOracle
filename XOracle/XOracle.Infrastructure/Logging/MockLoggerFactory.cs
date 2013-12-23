using System.Threading.Tasks;

using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class MockLoggerFactory : ILoggerFactory
    {
        public async Task<ILogger> Create()
        {
            return new MockLogger();
        }
    }
}
