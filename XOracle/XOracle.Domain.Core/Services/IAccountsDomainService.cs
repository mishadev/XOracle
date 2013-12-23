using System.Threading.Tasks;

namespace XOracle.Domain.Core.Services
{
    public interface IAccountsDomainService
    {
        Task<EventCreateResponse> CreateAccount(EventCreateRequest request);
    }
}
