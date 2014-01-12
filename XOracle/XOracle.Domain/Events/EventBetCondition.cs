using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class EventBetCondition : Entity
    {
        public Guid AccountId { get; set; }

        public DateTime CloseDate { get; set; }

        public Guid EventBetRateAlgorithmId { get; set; }

        public Guid CurrencyTypeId { get; set; }
    }
}
