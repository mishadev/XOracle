using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Azure.Core.Helpers;
using XOracle.Data;
using XOracle.Data.Azure;
using XOracle.Data.Azure.Entities;
using XOracle.Domain;

namespace XOracle.Azure.Web.Front
{
    public class AccountStore : IUserLoginStore<IdentityAccount>
    {
        private IAccountingService _accountingService;

        public AccountStore()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");

            this._accountingService = new AccountingService(
                new AzureRepository<AzureAccount, Account>(account),
                new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                new AzureRepository<AzureAccountBalance, AccountBalance>(account),
                new AzureRepository<AzureAccountLogin, AccountLogin>(account),
                new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                new AccountsFactory(
                    new AzureRepository<AzureAccount, Account>(account),
                    new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                    new AzureRepository<AzureAccountBalance, AccountBalance>(account),
                    new AzureRepository<AzureAccountLogin, AccountLogin>(account),
                    new AzureRepository<AzureAccountSet, AccountSet>(account),
                    new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                    new AzureScopeableUnitOfWorkFactory()));
        }

        public async Task CreateAsync(IdentityAccount user)
        {
            await _accountingService.CreateAccount(new CreateAccountRequest { Name = user.UserName });
        }

        public async Task DeleteAsync(IdentityAccount user)
        {
            await _accountingService.DeleteAccount(new DeleteAccountRequest { AccountId = Guid.Parse(user.Id) });
        }

        public async Task<IdentityAccount> FindByIdAsync(string userId)
        {
            var response = await _accountingService.GetAccount(new GetAccountRequest { AccountId = Guid.Parse(userId) });
            if (response != null)
                return new IdentityAccount { Id = response.AccountId.ToString(), UserName = response.Name };

            return null;
        }

        public async Task<IdentityAccount> FindByNameAsync(string userName)
        {
            var response = await _accountingService.GetAccount(new GetAccountRequest { Name = userName });
            if (response.AccountId != Guid.Empty)
                return new IdentityAccount { Id = response.AccountId.ToString(), UserName = response.Name };

            return null;
        }

        public async Task UpdateAsync(IdentityAccount user) { }

        public async Task AddLoginAsync(IdentityAccount user, UserLoginInfo login)
        {
            await this._accountingService.CreateAccountLogin(new CreateAccountLoginRequest { AccountId = Guid.Parse(user.Id), LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
        }

        public async Task<IdentityAccount> FindAsync(UserLoginInfo login)
        {
            var accountLogin = await this._accountingService.GetAccountLogin(new GetAccountLoginRequest { LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });
            if (accountLogin != null)
                return await this.FindByIdAsync(accountLogin.AccountId.ToString());

            return null;
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityAccount user)
        {
            var accountLogins = await this._accountingService.GetAccountLogins(new GetAccountLoginsRequest { AccountId = Guid.Parse(user.Id) });

            return accountLogins.AccountLoginResponses.Select(r => new UserLoginInfo(r.LoginProvider, r.ProviderKey)).ToList();
        }

        public Task RemoveLoginAsync(IdentityAccount user, UserLoginInfo login)
        {
            throw new InvalidOperationException("RemoveLoginAsync is not valid");
        }

        public void Dispose()
        { }
    }
}