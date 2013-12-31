using System;

namespace XOracle.Domain.Core
{
    public partial class EventBetCondition : Entity
    {
        public DateTime CloseDate { get; set; }

        public Guid EventBetRateAlgorithmId { get; set; }

        public Guid CurrencyTypeId { get; set; }
    }
}
