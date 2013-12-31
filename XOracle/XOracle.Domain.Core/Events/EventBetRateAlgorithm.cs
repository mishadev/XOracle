using System;

namespace XOracle.Domain.Core
{
    public partial class EventBetRateAlgorithm : Entity
    {
        public double StartRate { get; set; }

        public double EndRate { get; set; }

        public double LocusRage { get; set; }

        public Guid AlgorithmTypeId { get; set; }
    }
}
