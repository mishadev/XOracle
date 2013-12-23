﻿using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public static class LoggerFactory
    {
        static ILoggerFactory _factory = null;

        public static void SetCurrent(ILoggerFactory factory)
        {
            _factory = factory;
        }

        public static async Task<ILogger> Create()
        {
            if (_factory != null)
                return await _factory.Create();

            return null;
        }
    }
}
