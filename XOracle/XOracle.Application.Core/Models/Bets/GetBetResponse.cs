using System;

namespace XOracle.Application.Core
{
    public class GetBetResponse
    {
        public Guid AccountId { get; set; }

        public Guid EventId { get; set; }

        public string CurrencyType { get; set; }

        public decimal Value { get; set; }

        public string OutcomesType { get; set; }

        public DateTime CreationDate { get; set; }
    }

    public class GetBetResponseFirst : GetBetResponse
    {
        public GetAccountResponse Account { get; set; }

        public GetEventResponse Event { get; set; }
    }
}
