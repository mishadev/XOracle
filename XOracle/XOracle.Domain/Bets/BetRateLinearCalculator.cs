using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public class BetRateLinearCalculator : ICalculator<double, DateTime>
    {
        private double _start;
        private double _end;
        private double _locus;
        private DateTime _zero;
        private DateTime _max;

        public BetRateLinearCalculator(double start, double end, double locus, DateTime zero, DateTime max)
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

            return Math.Max(0, left / total * this._locus);
        }
    }
}