using System;

namespace XOracle.Domain.Core
{
    public class LoginResponse
    {
        public Guid AccountId { get; set; }

        public Guid Ticket { get; set; }
    }
}
