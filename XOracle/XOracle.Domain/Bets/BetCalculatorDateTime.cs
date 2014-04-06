using System;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public class BetRateCalculatorDateTime : ICalculator<double, DateTime>
    {
        private ICalculator<double, double> _calculator;

        private DateTime _min;
        private DateTime _max;

        public BetRateCalculatorDateTime(ICalculator<double, double> calculator, DateTime min, DateTime max)
        {
            this._calculator = calculator;

            this._min = min;
            this._max = max;
        }

        public double Calculate(DateTime value)
        {
            var total = (this._max - this._min).TotalMilliseconds;
            var left = (this._max - value).TotalMilliseconds;

            return this._calculator.Calculate(left / total);
        }
    }
}
