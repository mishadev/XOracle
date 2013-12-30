using System;
using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface IScopeable<T> : IDisposable
    { 
        Task Rollback();
    }
}
