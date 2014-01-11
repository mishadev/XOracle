using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureBetRateAlgorithm : TableServiceEntity
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

        public Guid AccountId { get; set; }

        public double StartRate { get; set; }

        public double EndRate { get; set; }

        public double LocusRage { get; set; }

        public Guid AlgorithmTypeId { get; set; }
    }
}
