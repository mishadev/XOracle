﻿using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface IFactory<T> : IFactory 
        where T : class
    {
        T Create();
    }

    public interface IFactory
    { }
}
