using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core.Logging
{
    public interface ILoggerFactory
    {
        Task<ILogger> Create();
    }
}
