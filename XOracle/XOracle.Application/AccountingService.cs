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
        private IRepository<Account> _repositoryAccount;
        private IRepository<CurrencyType> _repositoryCurrencyType;
        private IRepository<AccountBalance> _repositoryAccountBalance;
        private IRepository<AccountLogin> _repositoryAccountLogin;

        public AccountingService(
            IRepository<Account> repositoryAccount,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AccountBalance> repositoryAccountBalance,
            IRepository<AccountLogin> repositoryAccountLogin,
            IAccountsFactory accountsFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAccountBalance = repositoryAccountBalance;
            this._repositoryAccountLogin = repositoryAccountLogin;

            this._accountsFactory = accountsFactory;
        }

        public async Task<GetAccountResponse> GetAccount(GetAccountRequest request)
        {
            var name = request.Name;
            var account = request.AccountId.HasValue ?
                await this._repositoryAccount.Get(request.AccountId.Value) :
                await this._repositoryAccount.GetBy(a => a.Name == name);

            var response = new GetAccountResponse();
            if (account != null)
            {
                var currencyType = await this._repositoryCurrencyType.GetBy(v => v.Name == CurrencyType.Reputation);
                var balance = await this._accountsFactory.GetOrCreateBalance(account, currencyType);

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
            var account = await this._repositoryAccount.Get(request.AccountId);

            await this._repositoryAccount.Remove(account);

            return new DeleteAccountResponse { };
        }

        public async Task<GetAccountLoginResponse> GetAccountLogin(GetAccountLoginRequest request)
        {
            var loginProvider = request.LoginProvider;
            var providerKey = request.ProviderKey;

            AccountLogin accountlogin = request.AccountLoginId.HasValue ?
                await this._repositoryAccountLogin.Get(request.AccountLoginId.Value) :
                await this._repositoryAccountLogin.GetBy(al => al.LoginProvider == loginProvider && al.ProviderKey == providerKey);

            return this.ConvertAccountLogin(accountlogin);
        }

        public async Task<CreateAccountLoginResponse> CreateAccountLogin(CreateAccountLoginRequest request)
        {
            var accountLogin = await this._accountsFactory.CreateAccountLogin(request.AccountId, request.LoginProvider, request.ProviderKey);

            return new CreateAccountLoginResponse { AccountLoginId = accountLogin.Id };
        }

        public async Task<GetAccountLoginsResponse> GetAccountLogins(GetAccountLoginsRequest request)
        {
            var accountId = request.AccountId;
            IEnumerable<AccountLogin> accountlogins = await this._repositoryAccountLogin.GetFiltered(al => al.AccountId == accountId);

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
