using System;
using XOracle.Data.Core;

namespace XOracle.Application.Core
{
    public class GetEventRequest
    {
        public Guid EventId { get; set; }

        public DetalizationLevel DetalizationLevel { get; set; }

        public Guid AccountId { get; set; }
    }
}
