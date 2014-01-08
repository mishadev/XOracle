using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class AccountLogin : Entity
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public Guid AccountId { get; set; }
    }
}
