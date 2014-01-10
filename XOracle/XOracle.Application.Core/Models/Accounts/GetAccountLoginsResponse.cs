using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class GetAccountLoginsResponse
    {
        public IEnumerable<GetAccountLoginResponse> AccountLoginResponses { get; set; }
    }
}
