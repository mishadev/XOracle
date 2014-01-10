using XOracle.Data.Core;

namespace XOracle.Application.Core
{
    public interface IAppService
    {
        IRepositoryFactory Repositories { get; }
    }
}
