using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccountLogin : TableServiceEntity
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

        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
