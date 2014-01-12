using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public partial class AzureOutcomesType : TableServiceEntity
    {
        private Guid _id;

        public AzureOutcomesType()
        {
            this.PartitionKey = this.GetType().Name;
        }

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

        public string Name { get; set; }
    }
}
