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
            Repository<ValueType> r = new Repository<ValueType>(unit);

            await r.Add(new ValueType { Name = ValueType.ReputationName });
            await unit.Commit();
        }
    }
}
