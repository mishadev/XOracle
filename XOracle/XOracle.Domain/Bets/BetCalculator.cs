using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public abstract class BetRateCalculator : ICalculator<double, double>
    {
        protected double _locus;

        private double _minValue;
        private double _maxValue;

        public BetRateCalculator(double locus)
        {
            this._locus = locus;

            this._minValue = Calculate(0.0);
            this._maxValue = Calculate(1.0);
        }

        public double Calculate(double procentec)
        {
            double value = this.CalculateInner(procentec);

            return this.Normalize(value);
        }

        private double Normalize(double value)
        {
            return Math.Abs(value / (this._maxValue - this._minValue));
        }

        protected abstract double CalculateInner(double procentec);
    }
}
