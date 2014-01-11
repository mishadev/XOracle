﻿using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Data.Azure.Entities
{
    public partial class AzureCurrencyType : TableServiceEntity
    {
        public override string PartitionKey
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public string Name { get; set; }
    }
}
