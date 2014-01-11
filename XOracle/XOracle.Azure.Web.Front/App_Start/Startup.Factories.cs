using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Azure.Web.Front
{
    public partial class Startup
    {
        public static void Startup_Factories()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());
        }
    }
}