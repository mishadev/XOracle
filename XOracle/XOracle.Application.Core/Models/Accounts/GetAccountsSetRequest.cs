using System;
using XOracle.Data.Core;

namespace XOracle.Application.Core
{
    public class GetAccountsSetRequest
    {
        public Guid AccountSetId { get; set; }

        public DetalizationLevel DetalizationLevel { get; set; }
    }
}
