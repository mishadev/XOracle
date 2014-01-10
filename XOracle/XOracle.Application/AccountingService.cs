using System.Collections.Generic;
using System.Linq;
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

        public IRepositoryFactory Repositories
        {
            get { return _repositories; }
        }

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

            var response = new GetAccountResponse();
            if (account != null)
            {
                var currencyType = await this._repositories.Get<CurrencyType>().GetBy(v => v.Name == CurrencyType.Reputation);
                var balance = await this._repositories.Get<AccountBalance>().GetBy(b => b.AccountId == account.Id && b.CurrencyTypeId == currencyType.Id);

                response.AccountId = account.Id;
                response.Email = account.Email;
                response.Name = account.Name;
                response.Reputation = balance.Value;
            }

            return response;
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
            AccountLogin accountlogin = request.AccountLoginId.HasValue ?
                await this._repositories.Get<AccountLogin>().Get(request.AccountLoginId.Value) :
                await this._repositories.Get<AccountLogin>().GetBy(al => al.LoginProvider == request.LoginProvider && al.ProviderKey == request.ProviderKey);

            return this.ConvertAccountLogin(accountlogin);
        }

        public async Task<CreateAccountLoginResponse> CreateAccountLogin(CreateAccountLoginRequest request)
        {
            var accountLogin = await this._accountsFactory.CreateAccountLogin(request.AccountId, request.LoginProvider, request.ProviderKey);

            return new CreateAccountLoginResponse { AccountLoginId = accountLogin.Id };
        }

        public async Task<GetAccountLoginsResponse> GetAccountLogins(GetAccountLoginsRequest request)
        {
            IEnumerable<AccountLogin> accountlogins = await this._repositories.Get<AccountLogin>().GetFiltered(al => al.AccountId == request.AccountId);

            return new GetAccountLoginsResponse { AccountLoginResponses = accountlogins.Select(this.ConvertAccountLogin) };
        }

        private GetAccountLoginResponse ConvertAccountLogin(AccountLogin accountLogin)
        {
            if (accountLogin == null) return null;

            return new GetAccountLoginResponse
            {
                AccountLoginId = accountLogin.Id,
                LoginProvider = accountLogin.LoginProvider,
                ProviderKey = accountLogin.ProviderKey,
                AccountId = accountLogin.AccountId
            };
        }
    }
}
