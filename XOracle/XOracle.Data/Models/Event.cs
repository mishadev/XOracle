using System;
using XOracle.Data.Core;

namespace XOracle.Data
{
    public partial class Event : Entity
    {
        public Guid CreatorAccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LockDate { get; set; } 

        public DateTime EndDate { get; set; }

        public Guid AccessibilityConditionId { get; set; }

        public Guid JudgingConditionId { get; set; }

        public Guid VictoryConditionId { get; set; }

        public Guid BetConditionId { get; set; }
    }
}
