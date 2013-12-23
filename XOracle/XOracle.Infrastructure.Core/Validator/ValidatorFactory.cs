using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public static class ValidatorFactory
    {
        static IValidatorFactory _factory = null;

        public static void SetCurrent(IValidatorFactory factory)
        {
            _factory = factory;
        }

        public static async Task<IValidator> Create()
        {
            if (_factory != null)
                return await _factory.Create();

            return null;
        }
    }
}
