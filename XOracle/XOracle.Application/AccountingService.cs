using System;
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
        private IRepository<AccountSetAccounts> _repositoryAccountSetAccounts;

        public AccountingService(
            IRepository<Account> repositoryAccount,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AccountBalance> repositoryAccountBalance,
            IRepository<AccountLogin> repositoryAccountLogin,
            IRepository<AccountSetAccounts> repositoryAccountSetAccounts,
            IAccountsFactory accountsFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAccountBalance = repositoryAccountBalance;
            this._repositoryAccountLogin = repositoryAccountLogin;
            this._repositoryAccountSetAccounts = repositoryAccountSetAccounts;

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
                response = await this.GetDetails(account, request.DetalizationLevel);

            return response;
        }

        private async Task<GetAccountResponse> GetDetails(Account account, DetalizationLevel level)
        {
            switch (level)
            {
                case DetalizationLevel.Second:
                case DetalizationLevel.Full:
                case DetalizationLevel.First:
                    return await FillFirstDetalization(account);
                default:
                    return FillBaseDetalization(account);
            }
        }

        private GetAccountResponse FillBaseDetalization(Account account, GetAccountResponse model = null)
        {
            model = model ?? new GetAccountResponse();

            model.AccountId = account.Id;
            model.Name = account.Name;
            model.Email = account.Email;

            return model;
        }

        private async Task<GetAccountResponseFirst> FillFirstDetalization(Account account, GetAccountResponseFirst model = null)
        {
            model = model ?? new GetAccountResponseFirst();

            FillBaseDetalization(account, model);

            var currencyType = await this._repositoryCurrencyType.GetBy(v => v.Name == CurrencyType.Reputation);
            var balance = await this._accountsFactory.GetOrCreateBalance(account, currencyType);

            model.Reputation = balance.Value;

            return model;
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

        public async Task<GetAccountsSetResponse> GetAccountsSet(GetAccountsSetRequest request)
        {
            var setId = request.AccountSetId;
            var accountSetAccounts = await this._repositoryAccountSetAccounts.GetFiltered(asa => asa.AccountSetId == setId);

            var details = await this.GetDetails(accountSetAccounts, request.DetalizationLevel);
            return details;
        }

        private async Task<GetAccountsSetResponse> GetDetails(IEnumerable<AccountSetAccounts> accountSetAccounts, DetalizationLevel level)
        {
            switch (level)
            {
                case DetalizationLevel.First:
                    return await FillFirstDetalization(accountSetAccounts, DetalizationLevel.None);
                case DetalizationLevel.Second:
                case DetalizationLevel.Full:
                    return await FillFirstDetalization(accountSetAccounts, DetalizationLevel.First);
                default:
                    return FillBaseDetalization(accountSetAccounts);
            }
        }

        private GetAccountsSetResponse FillBaseDetalization(IEnumerable<AccountSetAccounts> accountSetAccounts, GetAccountsSetResponse model = null)
        {
            model = model ?? new GetAccountsSetResponse();

            model.AccountIds = accountSetAccounts.Select(asa => asa.AccountId);
            model.AccountsSetId = accountSetAccounts.FirstOrDefault().AccountSetId;

            return model;
        }

        private async Task<GetAccountsSetResponseFirst> FillFirstDetalization(IEnumerable<AccountSetAccounts> accountSetAccounts, DetalizationLevel inner, GetAccountsSetResponseFirst model = null)
        {
            model = model ?? new GetAccountsSetResponseFirst();

            FillBaseDetalization(accountSetAccounts, model);

            var list = new List<GetAccountResponse>(accountSetAccounts.Count());
            foreach (var asa in accountSetAccounts)
                list.Add(await this.GetAccount(new GetAccountRequest { AccountId = asa.AccountId, DetalizationLevel = inner }));

            model.Accounts = list;

            return model;
        }

        public async Task<CreateAccountsSetResponse> CreateAccountsSet(CreateAccountsSetRequest request)
        {
            var account = await this._repositoryAccount.Get(request.CreatorAccountId);
            var accountset = await this._accountsFactory.CreateAccountSet(account, await this.GetByIds(request.AccountIds));

            return new CreateAccountsSetResponse { Id = accountset.Id };
        }

        private async Task<IEnumerable<Account>> GetByIds(IEnumerable<Guid> ids)
        {
            var list = new List<Account>(ids.Count());

            foreach (var id in ids)
                list.Add(await this._repositoryAccount.Get(id));

            return list;
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
