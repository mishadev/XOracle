﻿using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface ILoggerFactory
    {
        Task<ILogger> Create();
    }
}