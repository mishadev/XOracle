using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class AccountTransaction : Entity
    {
        public Guid AccountId { get; set; }

        public Guid ValueTypeId { get; set; }

        public decimal Value { get; set; }

        public DateTime Date { get; set; }
    }
}
