using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public static class Factory<TInstance>
        where TInstance : class
    {
        static IFactory<TInstance> _factory = null;

        public static void SetCurrent(IFactory<TInstance> factory)
        {
            _factory = factory;
        }

        public static async Task<TInstance> Create()
        {
            if (_factory != null)
                return await _factory.Create();

            return null;
        }
    }
}
