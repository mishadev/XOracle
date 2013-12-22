using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core.Logging
{
    public static class LoggerFactory
    {
        static ILoggerFactory _currentLogFactory = null;

        public static void SetCurrent(ILoggerFactory logFactory)
        {
            _currentLogFactory = logFactory;
        }

        public static async Task<ILogger> CreateLog()
        {
            if (_currentLogFactory != null)
                return await _currentLogFactory.Create();

            return null;
        }
    }
}
