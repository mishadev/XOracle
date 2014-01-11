using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class CurrencyType : Entity, IEnum
    {
        public const string Reputation = "Reputation";

        public string Name { get; set; }
    }
}
