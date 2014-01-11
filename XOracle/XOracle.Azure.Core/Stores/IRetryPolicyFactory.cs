using Microsoft.Practices.TransientFaultHandling;

namespace XOracle.Azure.Core.Stores
{
    public interface IRetryPolicyFactory
    {
        RetryPolicy GetDefaultAzureStorageRetryPolicy();
    }
}
