using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class DataAnnotationsEntityValidatorFactory : IValidatorFactory
    {
        public async Task<IValidator> Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
