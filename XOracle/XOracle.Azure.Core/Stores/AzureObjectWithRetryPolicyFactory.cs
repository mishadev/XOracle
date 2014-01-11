using Microsoft.Practices.TransientFaultHandling;
using XOracle.Azure.Core.Helpers;
using XOracle.Infrastructure.Core;

namespace XOracle.Azure.Core.Stores
{
    public abstract class AzureObjectWithRetryPolicyFactory : IAzureObjectWithRetryPolicyFactory
    {
        public IRetryPolicyFactory RetryPolicyFactory { get; set; }

        public IRetryPolicyFactory GetRetryPolicyFactoryInstance()
        {
            return this.RetryPolicyFactory ?? new DefaultRetryPolicyFactory();
        }

        protected virtual void RetryPolicyTrace(object sender, RetryingEventArgs args)
        {
            var msg = string.Format(
                 "{0} Retry - Count:{1}, Delay:{2}, Exception:{3}",
                 this.GetType().Name,
                 args.CurrentRetryCount,
                 args.Delay,
                 args.LastException.TraceInformation());

            Factory<ILogger>.GetInstance().LogWarning(msg);
        }
    }
}
