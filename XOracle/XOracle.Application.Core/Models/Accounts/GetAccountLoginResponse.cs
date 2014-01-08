using System;

namespace XOracle.Application.Core
{
    public class GetAccountLoginResponse
    {
        public Guid AccountLoginId { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public Guid AccountId { get; set; }
    }
}
