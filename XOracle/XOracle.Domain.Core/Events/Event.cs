using System;

namespace XOracle.Domain.Core
{
    public partial class Event : Entity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CreatorAccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime LockDate { get; set; } 

        public DateTime EndDate { get; set; }

        public Guid ImageId { get; set; }

        public Guid EventRelationTypeId { get; set; }

        public Guid ParticipantsAccountSetId { get; set; }

        public Guid JudgingAccountSetId { get; set; }

        public Guid ExpectedEventConditionId { get; set; }

        public Guid RealEventConditionId { get; set; }

        public Guid EventBetConditionId { get; set; }
    }
}
