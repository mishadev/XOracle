using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountBalance : TableServiceEntity
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

        public Guid CurrencyTypeId { get; set; }

        public decimal Value { get; set; }
    }
}
