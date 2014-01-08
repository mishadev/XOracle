using System;

namespace XOracle.Domain
{
    public class BetRate
    {
        public DateTime CreationDate { get; set; }

        public decimal Rate { get; set; }

        public decimal PossibleWinValue { get; set; }
    }
}
