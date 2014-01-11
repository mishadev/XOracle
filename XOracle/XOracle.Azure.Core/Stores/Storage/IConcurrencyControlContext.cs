namespace XOracle.Azure.Core.Stores.Storage
{
    public interface IConcurrencyControlContext
    {
        string ObjectId { get; }
    }
}
