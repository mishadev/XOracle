using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountLogin : TableServiceEntity
    {
        public override string PartitionKey
        {
            get
            {
                return this.AccountId.ToString();
            }
            set
            {
                this.AccountId = Guid.Parse(value);
            }
        }

        public Guid AccountId { get; set; }

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
