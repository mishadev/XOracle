using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface IValidatorFactory
    {
        Task<IValidator> Create();
    }
}
