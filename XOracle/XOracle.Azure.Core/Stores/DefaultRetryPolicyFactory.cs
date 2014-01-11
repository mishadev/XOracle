using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.AzureStorage;
using Microsoft.Practices.TransientFaultHandling;

namespace XOracle.Azure.Core.Stores
{
    public class DefaultRetryPolicyFactory : IRetryPolicyFactory
    {
        public RetryPolicy GetDefaultAzureStorageRetryPolicy()
        {
            return new RetryPolicy(new StorageTransientErrorDetectionStrategy(), 3);
        }
    }
}
