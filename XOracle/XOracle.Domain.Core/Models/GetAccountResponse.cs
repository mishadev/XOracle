using System;

namespace XOracle.Domain.Core
{
    public class GetAccountResponse
    {
        public bool HasAccount { get; set; }

        public Guid AccountId { get; set; }
    }
}
