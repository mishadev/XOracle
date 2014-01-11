using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public partial class AzureEventBetCondition : TableServiceEntity
    {
        public override string PartitionKey
        {
            get
            {
                return this.EventId.ToString();
            }
            set
            {
                this.EventId = Guid.Parse(value);
            }
        }

        public Guid EventId { get; set; }

        public DateTime CloseDate { get; set; }

        public Guid EventBetRateAlgorithmId { get; set; }

        public Guid CurrencyTypeId { get; set; }
    }
}
