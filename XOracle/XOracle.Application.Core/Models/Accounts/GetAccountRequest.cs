using System;

namespace XOracle.Application.Core
{
    public class GetAccountRequest
    {
        public Guid? AccountId { get; set; }

        public string Name { get; set; }
    }
}
