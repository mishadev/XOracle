using System.Threading.Tasks;
using System.Linq;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain.Services
{
    public class AccountsDomainService : IAccountsDomainService
    {
        private IRepository<Account> _accountRepository;
        private IRepository<Reputation> _reputationRepository;

        public AccountsDomainService(IRepository<Account> accountRepository, IRepository<Reputation> reputationRepository)
        {
            this._accountRepository = accountRepository;
            this._reputationRepository = reputationRepository;
        }

        public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            var account = new Account { EMail = request.EMail };
            await this._accountRepository.Add(account);

            var reputation = new Reputation { AccountId = account.Id, Value = 1 };
            await this._reputationRepository.Add(reputation);

            return new CreateAccountResponse { EMail = account.EMail, AccountId = account.Id };
        }

        public async Task<GetAccountResponse> GetAccount(GetAccountRequest request)
        {
            Account account = (await this._accountRepository.GetAll()).FirstOrDefault(a => a.EMail.Equals(request.EMail, System.StringComparison.OrdinalIgnoreCase));

            var getAccountResponse = new GetAccountResponse { HasAccount = account != null };
            if(account != null)
                getAccountResponse.AccountId = account.Id;

            return getAccountResponse;
        }
    }
}
