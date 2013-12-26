using System;

namespace XOracle.Application.Core
{
    public class SingInResponse
    {
        public Guid AccountId { get; set; }

        public Guid Ticket { get; set; }
    }
}
