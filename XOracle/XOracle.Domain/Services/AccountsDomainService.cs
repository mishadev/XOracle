using System.Threading.Tasks;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;

namespace XOracle.Domain.Services
{
    public class AccountsDomainService : IAccountsDomainService
    {
        private IRepository<Account> _repository;

        public AccountsDomainService(IRepository<Account> repository)
        {
            this._repository = repository;
        }

        public async Task<EventCreateResponse> CreateAccount(EventCreateRequest request)
        {
            return new EventCreateResponse();
        }
    }
}
