using System;

namespace XOracle.Application.Core
{
    public class GetBetsDetailsResponse
    {
        public Guid AccountId { get; set; }

        public Guid EventId { get; set; }

        public string CurrencyType { get; set; }

        public decimal Value { get; set; }

        public string OutcomesType { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
