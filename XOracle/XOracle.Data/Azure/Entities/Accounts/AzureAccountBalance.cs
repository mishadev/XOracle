using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountBalance : TableServiceEntity
    {
        private Guid _accountId;
        private Guid _id;

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

        public Guid CurrencyTypeId { get; set; }

        public decimal Value { get; set; }
    }
}
