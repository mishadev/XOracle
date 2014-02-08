using System;
using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class GetEventResponse
    {
        public Guid EventId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid AccountId { get; set; }

        public Guid ImageId { get; set; }
        
        public Guid EventRelationTypeId { get; set; }
        
        public Guid ParticipantsAccountSetId { get; set; }
        
        public Guid ArbiterAccountSetId { get; set; }
        
        public Guid ExpectedEventConditionId { get; set; }
        
        public Guid RealEventConditionId { get; set; }
        
        public Guid EventBetConditionId { get; set; }
    }

    public class GetEventResponseFirst : GetEventResponse
    {
        public string RealEventCondition { get; set; }

        public string ExpectedEventCondition { get; set; }
    }

    public class GetEventResponseSecond : GetEventResponseFirst
    {
        public IEnumerable<GetAccountResponse> ArbiterAccounts { get; set; }

        public IEnumerable<GetAccountResponse> ParticipantsAccounts { get; set; }
    }

    public class GetEventResponseFull : GetEventResponseSecond
    {
        public CalculateBetRateResponse HappenBetRate { get; set; }

        public CalculateBetRateResponse NotHappenBetRate { get; set; }
    }
}
