namespace XOracle.Azure.Core.Stores
{
    public interface IAzureObjectWithRetryPolicyFactory
    {
        IRetryPolicyFactory RetryPolicyFactory { get; set; }

        IRetryPolicyFactory GetRetryPolicyFactoryInstance();
    }
}
