using System;
using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface ILogger
    {
        Task Debug(string message, params object[] args);

        Task Debug(string message, Exception exception, params object[] args);

        Task Debug(object item);

        Task Fatal(string message, params object[] args);

        Task Fatal(string message, Exception exception, params object[] args);

        Task LogInfo(string message, params object[] args);

        Task LogWarning(string message, params object[] args);

        Task LogError(string message, params object[] args);

        Task LogError(string message, Exception exception, params object[] args);
    }
}
