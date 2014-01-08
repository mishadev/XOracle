using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class AccountSet : Entity
    {
        public Guid AccountId { get; set; }
    }

    public partial class AccountSetAccounts : Entity
    {
        public Guid AccountSetId { get; set; }

        public Guid AccountId { get; set; }
    }
}
