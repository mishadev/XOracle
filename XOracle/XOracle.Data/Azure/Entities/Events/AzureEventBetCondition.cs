using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public partial class AzureEventBetCondition : TableServiceEntity
    {
        private Guid _id;
        private Guid _accountId;

        [RowKeyAttribute]
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

        [PartitionKeyAttribute]
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

        public DateTime CloseDate { get; set; }

        public Guid EventBetRateAlgorithmId { get; set; }

        public Guid CurrencyTypeId { get; set; }
    }
}
