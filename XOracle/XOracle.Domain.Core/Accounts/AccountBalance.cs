using System;
using XOracle.Domain.Core;

namespace XOracle.Domain.Core
{
    public partial class AccountBalance : Entity
    {
        public Guid AccountId { get; set; }

        public Guid ValueTypeId { get; set; }

        public decimal Value { get; set; }
    }
}
