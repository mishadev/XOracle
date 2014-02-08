using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class Event : Entity
    {
        public string Title { get; set; }

        public Guid AccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid ImageId { get; set; }

        public Guid EventRelationTypeId { get; set; }

        public Guid ParticipantsAccountSetId { get; set; }

        public Guid ArbiterAccountSetId { get; set; }

        public Guid ExpectedEventConditionId { get; set; }

        public Guid RealEventConditionId { get; set; }

        public Guid EventBetConditionId { get; set; }
    }
}
