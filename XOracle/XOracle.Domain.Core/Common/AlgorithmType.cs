using System;

namespace XOracle.Domain.Core
{
    public partial class AlgorithmType : Entity, IEnum
    {
        public static string Exponential = "Exponential";
        public static string Linear = "Lnear";

        public string Name { get; set; }
    }
}
