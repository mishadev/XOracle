using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class OutcomesType : Entity, IEnum
    {
        public static string Happen = "Happen";
        public static string NotHappen = "NotHappen";

        public string Name { get; set; }
    }
}
