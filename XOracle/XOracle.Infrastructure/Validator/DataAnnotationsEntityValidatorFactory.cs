using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class DataAnnotationsEntityValidatorFactory : IFactory<IValidator>
    {
        public IValidator Create()
        {
            return new DataAnnotationsEntityValidator();
        }
    }
}
