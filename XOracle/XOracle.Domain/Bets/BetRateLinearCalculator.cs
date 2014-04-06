using System;

namespace XOracle.Domain
{
    public class BetRateLinearCalculator : BetRateCalculator
    {
        public BetRateLinearCalculator(double locus)
            : base(locus)
        { }

        protected override double CalculateInner(double x)
        {
            return Math.Max(0, x * this._locus);
        }
    }
}