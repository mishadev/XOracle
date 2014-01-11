using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class MockLoggerFactory : IFactory<ILogger>
    {
        public ILogger Create()
        {
            return new MockLogger();
        }
    }
}
