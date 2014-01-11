using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class TraceLoggerFactory : IFactory<ILogger>
    {
        public ILogger Create()
        {
            return new TraceLogger();
        }
    }
}
