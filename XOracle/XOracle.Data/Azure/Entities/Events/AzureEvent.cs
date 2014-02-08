using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureEvent : TableServiceEntity
    {
        private Guid _accountId;
        private Guid _id;

        [RowKey]
        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
                this.RowKey = this._id.ToString();
            }
        }

        [PartitionKey]
        public Guid AccountId
        {
            get
            {
                return this._accountId;
            }
            set
            {
                this._accountId = value;
                this.PartitionKey = this._accountId.ToString();
            }
        }

        public string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public Guid ImageId { get; set; }

        public Guid EventRelationTypeId { get; set; }

        public Guid ParticipantsAccountSetId { get; set; }

        public Guid ArbiterAccountSetId { get; set; }

        public Guid ExpectedEventConditionId { get; set; }

        public Guid RealEventConditionId { get; set; }

        public Guid EventBetConditionId { get; set; }
    }
}
