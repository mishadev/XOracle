using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class CurrencyType : Entity, IEnum
    {
        public static string Reputation = "Reputation";

        public string Name { get; set; }
    }
}
