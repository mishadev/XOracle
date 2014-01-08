using System;

namespace XOracle.Application.Core
{
    public class GetAccountResponse
    {
        public Guid AccountId { get; set; }

        public decimal Reputation { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }
    }
}
