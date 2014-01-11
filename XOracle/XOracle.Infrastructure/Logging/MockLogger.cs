using System.Threading.Tasks;

using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    internal class MockLogger : ILogger
    {
        public void LogInfo(string message, params object[] args)
        { }

        public void LogWarning(string message, params object[] args)
        { }

        public void LogError(string message, params object[] args)
        { }

        public void LogVerbose(string message, params object[] args)
        { }
    }
}
