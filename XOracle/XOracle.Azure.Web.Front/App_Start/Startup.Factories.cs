using System.Threading.Tasks;
using XOracle.Azure.Core.Helpers;
using XOracle.Data.Azure;
using XOracle.Data.Azure.Entities;
using XOracle.Domain;
using XOracle.Domain.Core;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Azure.Web.Front
{
    public partial class Startup
    {
        public static void Startup_Factories()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new TraceLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());
        }

        public static void Startup_Enums()
        {
            Task.Run(async () =>
            {
                var account = CloudConfiguration.GetStorageAccount("DataConnectionString");
                AzureRepository<AzureCurrencyType, CurrencyType> valueTypeRepo = new AzureRepository<AzureCurrencyType, CurrencyType>(account);
                AzureRepository<AzureEventRelationType, EventRelationType> eventRelationTypeRepo = new AzureRepository<AzureEventRelationType, EventRelationType>(account);
                AzureRepository<AzureAlgorithmType, AlgorithmType> algorithmTypeRepo = new AzureRepository<AzureAlgorithmType, AlgorithmType>(account);
                AzureRepository<AzureOutcomesType, OutcomesType> outcomesTypeRepo = new AzureRepository<AzureOutcomesType, OutcomesType>(account);

                Entity enums = await valueTypeRepo.GetBy(v => v.Name == CurrencyType.Reputation);
                if (enums == null)
                    await valueTypeRepo.Add(new CurrencyType { Name = CurrencyType.Reputation });

                enums = await eventRelationTypeRepo.GetBy(v => v.Name == EventRelationType.OneVsOne);
                if (enums == null)
                {
                    await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsOne });
                    await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsMeny });
                    await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.MenyVsMeny });
                }

                enums = await algorithmTypeRepo.GetBy(v => v.Name == AlgorithmType.Exponential);
                if (enums == null)
                {
                    await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Exponential });
                    await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Linear });
                }

                enums = await algorithmTypeRepo.GetBy(v => v.Name == OutcomesType.Happen);
                if (enums == null)
                {
                    await outcomesTypeRepo.Add(new OutcomesType { Name = OutcomesType.Happen });
                    await outcomesTypeRepo.Add(new OutcomesType { Name = OutcomesType.NotHappen });
                }
                
            });
        }
    }
}