using System;

namespace XOracle.Domain
{
    public class BetRateExponentialCalculator : BetRateCalculator
    {
        public BetRateExponentialCalculator(double locus)
            : base(locus)
        { }

        protected override double CalculateInner(double x)
        {
            return Math.Pow(x, this._locus * 3);
        }
    }
}