using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class AlgorithmType : Entity, IEnum
    {
        public const string Exponential = "Exponential";
        public const string Linear = "Lnear";

        public string Name { get; set; }
    }
}
