using System;
using XOracle.Data.Core;

namespace XOracle.Data
{
    [Serializable]
    public class Reputation : Entity
    {
        public Guid AccountId { get; set; }

        public double Value { get; set; }
    }
}
