using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureEvent : TableServiceEntity
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

        public string Title { get; set; }

        public Guid AccountId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid ImageId { get; set; }

        public Guid EventRelationTypeId { get; set; }

        public Guid ParticipantsAccountSetId { get; set; }

        public Guid JudgingAccountSetId { get; set; }

        public Guid ExpectedEventConditionId { get; set; }

        public Guid RealEventConditionId { get; set; }

        public Guid EventBetConditionId { get; set; }
    }
}
