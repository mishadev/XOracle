using System;
using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class CreateAccountsSetRequest
    {
        public Guid CreatorAccountId { get; set; }

        public IEnumerable<Guid> AccountIds { get; set; }
    }
}
