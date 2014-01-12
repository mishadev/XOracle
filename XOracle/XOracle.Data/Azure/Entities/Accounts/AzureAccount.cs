using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccount : TableServiceEntity
    {
        private Guid _id;

        [PartitionKey]
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
                this.PartitionKey = this._id.GetHashCode().ToString(AzureEntityFactory.SELF_ID_PARTIOTION_FORMAT);
                this.RowKey = this._id.ToString();
            }
        }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
