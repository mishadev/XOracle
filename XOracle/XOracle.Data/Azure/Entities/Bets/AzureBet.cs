using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureBet : TableServiceEntity
    {
        private Guid _eventId;
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
        public Guid EventId
        {
            get
            {
                return this._eventId;
            }
            set
            {
                this._eventId = value;
                this.PartitionKey = this._eventId.ToString();
            }
        }

        public Guid CurrencyTypeId { get; set; }

        public double Value { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid AccountId { get; set; }

        public Guid OutcomesTypeId { get; set; }
    }
}
