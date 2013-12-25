using System;
using System.Threading.Tasks;

namespace XOracle.Data.Core
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit();

        Task Rollback();
    }
}
