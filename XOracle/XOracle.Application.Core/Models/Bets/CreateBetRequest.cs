using System;

namespace XOracle.Application.Core
{
    public class CreateBetRequest
    {
        public Guid EventId { get; set; }

        public Guid AccountId { get; set; }

        public string OutcomesType { get; set; }

        public decimal BetAmount { get; set; }
    }
}
