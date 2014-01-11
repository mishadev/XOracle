using Microsoft.Practices.TransientFaultHandling;

namespace XOracle.Azure.Core.Stores.Storage
{
    public abstract class AzureStorageWithRetryPolicy : AzureObjectWithRetryPolicyFactory
    {
        protected RetryPolicy StorageRetryPolicy
        {
            get
            {
                var retryPolicy = this.GetRetryPolicyFactoryInstance().GetDefaultAzureStorageRetryPolicy();
                retryPolicy.Retrying += this.RetryPolicyTrace;
                return retryPolicy;
            }
        }
    }
}
