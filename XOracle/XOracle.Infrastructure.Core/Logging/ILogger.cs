using System;
using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface ILogger
    {
        void LogInfo(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void LogError(string message, params object[] args);

        void LogVerbose(string message, params object[] args);
    }
}
