using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain;

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
            await valueTypeRepo.Add(new CurrencyType { Name = CurrencyType.Reputation });

            Repository<EventRelationType> eventRelationTypeRepo = new Repository<EventRelationType>(unit);
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsOne });
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.OneVsMeny });
            await eventRelationTypeRepo.Add(new EventRelationType { Name = EventRelationType.MenyVsMeny });

            Repository<AlgorithmType> algorithmTypeRepo = new Repository<AlgorithmType>(unit);
            await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Exponential });
            await algorithmTypeRepo.Add(new AlgorithmType { Name = AlgorithmType.Linear });

            Repository<OutcomesType> outcomesTypeRepo = new Repository<OutcomesType>(unit);
            await outcomesTypeRepo.Add(new OutcomesType { Name = OutcomesType.Happen });
            await outcomesTypeRepo.Add(new OutcomesType { Name = OutcomesType.NotHappen });

            await unit.Commit();
        }
    }
}
