using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class DataAnnotationsEntityValidatorFactory : IFactory<IValidator>
    {
        public async Task<IValidator> Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
