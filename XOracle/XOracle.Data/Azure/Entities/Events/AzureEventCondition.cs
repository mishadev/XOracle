using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureEventCondition : TableServiceEntity
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

        public string Description { get; set; }
    }
}
