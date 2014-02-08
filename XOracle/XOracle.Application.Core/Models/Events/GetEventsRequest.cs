using System;
using XOracle.Data.Core;

namespace XOracle.Application.Core
{
    public class GetEventsRequest
    {
        public Guid AccountId { get; set; }

        public DetalizationLevel DetalizationLevel { get; set; }
    }
}
