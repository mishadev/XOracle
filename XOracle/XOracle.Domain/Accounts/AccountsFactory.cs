using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain
{
    public interface IAccountsFactory
    {
        Task<Account> CreateAccount(string email, string name);

        Task<AccountLogin> CreateAccountLogin(Guid accountId, string loginProvider, string providerKey);

        Task<AccountBalance> CreateAccountBalance(Account account, CurrencyType currencyType, decimal value);
    }

    public class AccountsFactory : IAccountsFactory
    {
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;
        private IRepositoryFactory _repositories;

        public AccountsFactory(
            IRepositoryFactory repositories,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositories = repositories;
            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Account> CreateAccount(string email, string name)
        {
            await this.Check(name);

            using (await this._scopeableFactory.Create())
            {                                                    
                var account = new Account { Email = email, Name = name ?? email.Substring(0, email.IndexOf('@')) };
                await this._repositories.Get<Account>().Add(account);

                CurrencyType currencyType = (await this._repositories.Get<CurrencyType>().GetBy(v => v.Name == CurrencyType.Reputation));

                await this.CreateAccountBalance(account, currencyType, 1);

                return account;
            }
        }

        private async Task Check(string name)
        {
            IEnumerable<Account> accounts = await this._repositories.Get<Account>().GetFiltered(a => a.Name == name);

            if (accounts.Any())
                throw new InvalidOperationException("email should be unique");
        }

        public async Task<AccountBalance> CreateAccountBalance(Account account, CurrencyType currencyType, decimal value)
        {
            using (await this._scopeableFactory.Create())
            {
                var balance = new AccountBalance { AccountId = account.Id, Value = 1, CurrencyTypeId = currencyType.Id };

                await this._repositories.Get<AccountBalance>().Add(balance);

                return balance;
            }
        }

        public async Task<AccountLogin> CreateAccountLogin(Guid accountId, string loginProvider, string providerKey)
        {
            using (await this._scopeableFactory.Create())
            {
                AccountLogin accountLogin = new AccountLogin
                {
                    AccountId = accountId,
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey
                };

                await this._repositories.Get<AccountLogin>().Add(accountLogin);

                return accountLogin;
            }
        }
    }
}
