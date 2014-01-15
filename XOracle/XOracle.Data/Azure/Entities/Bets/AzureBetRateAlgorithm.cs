using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureBetRateAlgorithm : TableServiceEntity
    {
        private Guid _id;
        private Guid _accountId;

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

        public double StartRate { get; set; }

        public double EndRate { get; set; }

        public double LocusRage { get; set; }

        public Guid AlgorithmTypeId { get; set; }
    }
}
