using Microsoft.WindowsAzure.StorageClient;

namespace XOracle.Data.Azure.Entities
{
    public class AzureAccount : TableServiceEntity
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
