using System;

namespace XOracle.Domain
{
    public class BetRateCalculatorFactory
    {
        private double _locus;

        public BetRateCalculatorFactory(double locus)
        {
            this._locus = locus;
        }

        public BetRateCalculatorDateTime CreateDateTime(string algorithmType, DateTime min, DateTime max)
        {
            BetRateCalculator calculator = this.Create(algorithmType);

            return new BetRateCalculatorDateTime(calculator, min, max);
        }

        public BetRateCalculator Create(string algorithmType)
        {
            if (algorithmType == AlgorithmType.Exponential)
                return new BetRateExponentialCalculator(this._locus);

            if (algorithmType == AlgorithmType.Linear)
                return new BetRateLinearCalculator(this._locus);

            throw new InvalidOperationException("Create");
        }
    }
}
