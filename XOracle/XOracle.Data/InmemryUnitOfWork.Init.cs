using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain.Core;

namespace XOracle.Data
{
    public partial class InmemoryUnitOfWork
    {
        static InmemoryUnitOfWork()
        {
            Init(new InmemoryUnitOfWork()).GetAwaiter().GetResult();
        }

        private static async Task Init(IUnitOfWork unit)
        {
            Repository<CurrencyType> valueTypeRepo = new Repository<CurrencyType>(unit);
            await valueTypeRepo.Add(new CurrencyType { Name = CurrencyType.ReputationName });

            Repository<EventRelationType> eventRelationTypeRepo = new Repository<EventRelationType>(unit);
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsOne });
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsMeny });
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.MenyVsMeny });

            Repository<AlgorithmType> algorithmTypeRepo = new Repository<AlgorithmType>(unit);
            await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Exponential });
            await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Linear });

            await unit.Commit();
        }
    }
}
