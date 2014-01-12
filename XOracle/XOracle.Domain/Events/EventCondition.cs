using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class EventCondition : Entity
    {
        public Guid AccountId { get; set; }

        public string Description { get; set; }
    }
}
