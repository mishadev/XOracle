using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Domain.Core.Services;

namespace XOracle.Application
{
    public class AccountsService : IAccountsService
    {
        private IAccountsDomainService _accountsService;

        public AccountsService(IAccountsDomainService accountsService)
        {
            this._accountsService = accountsService;
        }

        public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            return new CreateAccountResponse();
        }

        public async Task<GetAccountResponse> GetAccount(GetAccountRequest request)
        {
            return new GetAccountResponse();
        }
    }
}
