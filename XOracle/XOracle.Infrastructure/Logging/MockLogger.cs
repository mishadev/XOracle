using System;
using System.Threading.Tasks;

using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Logging
{
    internal class MockLogger : ILogger
    {
        public async Task Debug(string message, params object[] args) 
        { }

        public async Task Debug(string message, Exception exception, params object[] args) 
        { }

        public async Task Debug(object item) 
        { }

        public async Task Fatal(string message, params object[] args) 
        { }

        public async Task Fatal(string message, Exception exception, params object[] args) 
        { }

        public async Task LogInfo(string message, params object[] args)
        { }

        public async Task LogWarning(string message, params object[] args)
        { }

        public async Task LogError(string message, params object[] args)
        { }

        public async Task LogError(string message, Exception exception, params object[] args)
        { }
    }
}
