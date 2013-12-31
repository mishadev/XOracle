namespace XOracle.Domain.Core
{
    public partial class CurrencyType : Entity, IEnum
    {
        public static string ReputationName = "Reputation";

        public string Name { get; set; }
    }
}
