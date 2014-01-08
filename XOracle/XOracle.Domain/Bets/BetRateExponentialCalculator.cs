using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public class BetRateExponentialCalculator : ICalculator<double, DateTime>
    {
        private double _start;
        private double _end;
        private double _locus;
        private DateTime _zero;
        private DateTime _max;

        public BetRateExponentialCalculator(double start, double end, double locus, DateTime zero, DateTime max)
        {
            this._start = start;
            this._end = end;
            this._locus = locus;
            this._zero = zero;
            this._max = max;
        }

        public double Calculate(DateTime x)
        {
            var total = (this._max - this._zero).TotalMilliseconds;
            var left = (this._max - x).TotalMilliseconds;

            return Math.Pow(left / total, this._locus);
        }
    }
}