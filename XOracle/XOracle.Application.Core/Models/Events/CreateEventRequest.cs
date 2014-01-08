using System;
using System.Collections.Generic;
using System.Linq;

namespace XOracle.Application.Core
{
    public class CreateEventRequest
    {
        public Guid AccountId { get; set; }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string ImageUri { get; set; }

        public string EventRelationType { get; set; }

        IEnumerable<Guid> _participantsAccountIds;
        public IEnumerable<Guid> ParticipantsAccountIds
        {
            get
            {
                return this._participantsAccountIds ?? Enumerable.Empty<Guid>();
            }
            set 
            {
                this._participantsAccountIds = value;
            }
        }

        IEnumerable<Guid> _judgingAccountIds;
        public IEnumerable<Guid> JudgingAccountIds
        {
            get
            {
                return this._judgingAccountIds ?? Enumerable.Empty<Guid>();
            }
            set
            {
                this._judgingAccountIds = value;
            }
        }

        public string ExpectedEventCondition { get; set; }

        public string RealEventCondition { get; set; }

        public DateTime CloseDate { get; set; }

        public string CurrencyType { get; set; }

        public double StartRate { get; set; }

        public double EndRate { get; set; }

        public double LocusRage { get; set; }

        public string AlgorithmType { get; set; }
    }
}
