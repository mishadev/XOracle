namespace XOracle.Domain.Core
{
    public interface ICalculatorFactory<TOut, TIn>
    {
        ICalculator<TOut, TIn> Create(string algorithmType);
    }
}
