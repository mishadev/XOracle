using System;

namespace XOracle.Application.Core
{
    public class GetBetConditionsResponse
    {
        public DateTime CloseDate { get; set; }

        public byte[] BetRateChartData { get; set; }

        public string CurrencyType { get; set; }
    }
}
