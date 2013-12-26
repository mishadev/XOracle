using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface ITypeAdapter
    {
        Task<TTarget> Adapt<TSource, TTarget>(TSource source)
            where TTarget : class,new()
            where TSource : class;

        Task<TTarget> Adapt<TTarget>(object source)
            where TTarget : class,new();
    }
}
