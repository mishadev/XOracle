using System;
using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class GetAccountsSetResponse
    {
        public Guid AccountsSetId { get; set;}

        public IEnumerable<Guid> AccountIds { get; set; }
    }

    public class GetAccountsSetResponseFirst : GetAccountsSetResponse
    {
        public IEnumerable<GetAccountResponse> Accounts { get; set; }
    }
}
