namespace XOracle.Domain.Core
{
    public interface ICalculator<TOut, TIn>
    {
        TOut Calculate(TIn paramete);
    }
}
