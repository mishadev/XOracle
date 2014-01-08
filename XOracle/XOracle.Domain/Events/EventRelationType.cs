using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class EventRelationType : Entity, IEnum
    {
        public static string OneVsOne = "1..1";
        public static string OneVsMeny = "1..*";
        public static string MenyVsMeny = "*..*";

        public string Name { get; set; }
    }
}
