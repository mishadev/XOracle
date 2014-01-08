using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class AlgorithmType : Entity, IEnum
    {
        public static string Exponential = "Exponential";
        public static string Linear = "Lnear";

        public string Name { get; set; }
    }
}
