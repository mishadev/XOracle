using System;

namespace XOracle.Domain
{
    public class BetRate
    {
        public DateTime CreationDate { get; set; }

        public decimal Rate { get; set; }

        public decimal WinValue { get; set; }

        public decimal WinRate { get; set; }
    }
}
