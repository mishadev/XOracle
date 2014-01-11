using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Azure.Core.Stores.Storage
{
    public class OptimisticConcurrencyContext : IConcurrencyControlContext
    {
        public OptimisticConcurrencyContext()
        {
            this.AccessCondition = AccessCondition.None;
        }

        internal OptimisticConcurrencyContext(string entityTag)
        {
            this.AccessCondition = AccessCondition.IfMatch(entityTag);
        }

        public AccessCondition AccessCondition { get; set; }

        public string ObjectId { get; set; }
    }
}
