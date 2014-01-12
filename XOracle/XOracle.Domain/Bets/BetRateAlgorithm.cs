using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class BetRateAlgorithm : Entity
    {
        public Guid AccountId { get; set; }

        public double StartRate { get; set; }

        public double EndRate { get; set; }

        public double LocusRage { get; set; }

        public Guid AlgorithmTypeId { get; set; }
    }
}
