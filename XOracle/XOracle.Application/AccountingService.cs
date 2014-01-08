using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Application
{
    public class AccountingService : IAccountingService
    {
        private IAccountsFactory _accountsFactory;
        private IRepositoryFactory _repositories;

        public AccountingService(
            IRepositoryFactory repositories,
            IAccountsFactory accountsFactory)
        {
            this._repositories = repositories;
            this._accountsFactory = accountsFactory;
        }

        public async Task<GetAccountResponse> GetAccount(GetAccountRequest request)
        {
            var account = request.AccountId.HasValue ?
                await this._repositories.Get<Account>().Get(request.AccountId.Value) :
                await this._repositories.Get<Account>().GetBy(a => a.Name == request.Name);
            var currencyType = await this._repositories.Get<CurrencyType>().GetBy(v => v.Name == CurrencyType.Reputation);
            var balance = await this._repositories.Get<AccountBalance>().GetBy(b => b.AccountId == account.Id && b.CurrencyTypeId == currencyType.Id);

            return new GetAccountResponse { AccountId = account.Id, Email = account.Email, Name = account.Name, Reputation = balance.Value };
        }

        public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            var account = await this._accountsFactory.CreateAccount(request.Email, request.Name);

            return new CreateAccountResponse { AccountId = account.Id };
        }

        public async Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequest request)
        {
            var account = await this._repositories.Get<Account>().Get(request.AccountId);

            await this._repositories.Get<Account>().Remove(account);

            return new DeleteAccountResponse { };
        }

        public async Task<GetAccountLoginResponse> GetAccountLogin(GetAccountLoginRequest request)
        {
            AccountLogin accountlogin = await this._repositories.Get<AccountLogin>().Get(request.AccountLoginId);

            return new GetAccountLoginResponse { 
                AccountLoginId = accountlogin.Id, 
                LoginProvider = accountlogin.LoginProvider, 
                ProviderKey = accountlogin.ProviderKey,
                AccountId = accountlogin.AccountId};
        }

        public async Task<CreateAccountLoginResponse> CreateAccountLogin(CreateAccountLoginRequest request)
        {
            AccountLogin accountLogin = new AccountLogin { 
                AccountId = request.AccountId, 
                LoginProvider = request.LoginProvider, 
                ProviderKey = request.ProviderKey };

            await this._repositories.Get<AccountLogin>().Add(accountLogin);

            return new CreateAccountLoginResponse { AccountLoginId = accountLogin.Id };
        }
    }
}
