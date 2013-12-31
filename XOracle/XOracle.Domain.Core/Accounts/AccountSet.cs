using System;

namespace XOracle.Domain.Core
{
    public partial class AccountSet : Entity
    { }

    public partial class AccountSetAccounts : Entity
    {
        public Guid AccountSetId { get; set; }

        public Guid AccountId { get; set; }
    }
}
