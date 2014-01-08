using System;

namespace XOracle.Application.Core
{
    public class CreateAccountLoginRequest
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }

        public Guid AccountId { get; set; }
    }
}
