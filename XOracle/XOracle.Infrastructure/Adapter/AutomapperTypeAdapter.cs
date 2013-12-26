using AutoMapper;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class AutomapperTypeAdapter : ITypeAdapter
    {
        public async Task<TTarget> Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new()
        {
            return Mapper.Map<TSource, TTarget>(source);
        }

        public async Task<TTarget> Adapt<TTarget>(object source) where TTarget : class, new()
        {
            return Mapper.Map<TTarget>(source);
        }
    }
}
