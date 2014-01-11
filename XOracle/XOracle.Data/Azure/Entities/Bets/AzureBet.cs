using Microsoft.WindowsAzure.StorageClient;
using System;

namespace XOracle.Data.Azure.Entities
{
    public class AzureBet : TableServiceEntity
    {
        public override string PartitionKey
        {
            get
            {
                return this.AccountId.ToString() + '_' + this.EventId.ToString();
            }
            set
            {
                var splitted = value.Split('_');

                if (splitted.Length == 2)
                {
                    this.AccountId = Guid.Parse(splitted[0]);
                    this.EventId = Guid.Parse(splitted[1]);
                }
            }
        }

        public Guid EventId { get; set; }

        public Guid CurrencyTypeId { get; set; }

        public decimal Value { get; set; }

        public DateTime CreationDate { get; set; }

        public Guid AccountId { get; set; }

        public Guid OutcomesTypeId { get; set; }
    }
}
