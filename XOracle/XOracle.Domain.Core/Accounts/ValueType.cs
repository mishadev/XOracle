using XOracle.Domain.Core;

namespace XOracle.Domain.Core
{
    public partial class ValueType : Entity
    {
        public static string ReputationName = "Reputation";

        public string Name { get; set; }
    }
}
