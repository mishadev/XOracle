using System;

namespace XOracle.Domain.Core
{
    public class CreateAccountResponse
    {
        public Guid AccountId { get; set; }

        public string EMail { get; set; }
    }
}
