using System;
using System.Collections.Generic;

namespace XOracle.Infrastructure.Core
{
    public interface IValidator
    {
        bool IsValid<TEntity>(TEntity item)
            where TEntity : class;

        IEnumerable<string> GetErrorMessages<TEntity>(TEntity item)
            where TEntity : class;
    }
}
