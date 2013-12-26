using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Utils
{
    public class RegexEmailAddressValidatorFactory : IFactory<IEmailAddressValidator>
    {
        public async Task<IEmailAddressValidator> Create()
        {
            return new RegexEmailAddressValidator();
        }
    }
}
