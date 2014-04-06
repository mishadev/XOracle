using System;
using System.Collections.Generic;

namespace XOracle.Azure.Web.Front.Models
{
    public class EventBrieflyViewModel
    {
        public Guid EventId { get; set; }

        public string Title { get; set; }

        public string Condition { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int TimeToClose { get; set; }

        public IEnumerable<AccountViewModel> ArbiterAccountSet { get; set; }

        public BetRateViewModel HappenBetRate { get; set; }

        public BetRateViewModel NotHappenBetRate { get; set; }
    }
}