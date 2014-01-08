using System;
using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class GetEventsResponse
    {
        public IEnumerable<Guid> EventIds { get; set; }
    }
}
