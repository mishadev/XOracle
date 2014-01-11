using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Utils
{
    public class RegexEmailAddressValidatorFactory : IFactory<IEmailAddressValidator>
    {
        public IEmailAddressValidator Create()
        {
            return new RegexEmailAddressValidator();
        }
    }
}
