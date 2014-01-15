using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountSet : TableServiceEntity
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
    }

    public class AzureAccountSetAccounts : TableServiceEntity
    {
        private Guid _accountSetId;
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
        public Guid AccountSetId
        {
            get
            {
                return this._accountSetId;
            }
            set
            {
                this._accountSetId = value;
                this.PartitionKey = this._accountSetId.ToString();
            }
        }

        public Guid AccountId { get; set; }
    }
}
