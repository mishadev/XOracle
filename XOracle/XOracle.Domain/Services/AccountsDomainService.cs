using System.Threading.Tasks;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;

namespace XOracle.Domain.Services
{
    public class AccountsDomainService : IAccountsDomainService
    {
        public async Task<EventCreateResponse> CreateAccount(EventCreateRequest request)
        {
            return new EventCreateResponse();
        }
    }
}
