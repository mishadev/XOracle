using System;

namespace XOracle.Application
{
    public class CalculateBetRateRequest
    {
        public Guid EventId { get; set; }

        public Guid AccountId { get; set; }

        public string OutcomesType { get; set; }

        public decimal BetAmount { get; set; }
    }
}
