using System;

namespace XOracle.Application.Core
{
    public class GetBetsRequest
    {
        public Guid? AccountId { get; set; }

        public Guid? EventId { get; set; }
    }
}
