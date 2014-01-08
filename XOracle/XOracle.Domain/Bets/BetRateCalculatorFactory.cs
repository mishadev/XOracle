using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public class BetRateCalculatorFactory : ICalculatorFactory<double, DateTime>
    {
        private double _start;
        private double _end;
        private double _locus;
        private DateTime _zero;
        private DateTime _max;

        public BetRateCalculatorFactory(double start, double end, double locus, DateTime zero, DateTime max)
        {
            this._start = start;
            this._end = end;
            this._locus = locus;
            this._zero = zero;
            this._max = max;
        }

        public ICalculator<double, DateTime> Create(string algorithmType)
        {
            if (algorithmType == AlgorithmType.Exponential)
                return new BetRateExponentialCalculator(this._start, this._end, this._locus, this._zero, this._max);

            if (algorithmType == AlgorithmType.Linear)
                return new BetRateLinearCalculator(this._start, this._end, this._locus, this._zero, this._max);

            throw new InvalidOperationException("Create");
        }
    }
}
