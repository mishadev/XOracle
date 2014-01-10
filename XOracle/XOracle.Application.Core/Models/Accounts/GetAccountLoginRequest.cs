using System;
namespace XOracle.Application.Core
{
    public class GetAccountLoginRequest
    {
        public Guid? AccountLoginId { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
