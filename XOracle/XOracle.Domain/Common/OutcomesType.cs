using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class OutcomesType : Entity, IEnum
    {
        public const string Happen = "Happen";
        public const string NotHappen = "NotHappen";

        public string Name { get; set; }
    }
}
