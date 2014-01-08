using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class Bet : Entity
    {
        public Guid EventId { get; set; }

        public Guid CurrencyTypeId { get; set; }

        public decimal Value { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid AccountId { get; set; }

        public Guid OutcomesTypeId { get; set; }
    }
}
