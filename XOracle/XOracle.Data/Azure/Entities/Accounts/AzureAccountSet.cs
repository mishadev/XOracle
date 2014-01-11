using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountSet : TableServiceEntity
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
    }

    public class AzureAccountSetAccounts : TableServiceEntity
    {
        public override string PartitionKey
        {
            get
            {
                return this.AccountSetId.ToString();
            }
            set
            {
                this.AccountSetId = Guid.Parse(value);
            }
        }

        public Guid AccountSetId { get; set; }

        public Guid AccountId { get; set; }
    }
}
