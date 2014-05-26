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

        Task<AccountBalance> GetOrCreateBalance(Account account, CurrencyType currencyType);

        Task<AccountSet> CreateAccountSet(Account account, IEnumerable<Account> accounts);
    }

    public class AccountsFactory : IAccountsFactory
    {
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        private IRepository<Account> _repositoryAccount;
        private IRepository<CurrencyType> _repositoryCurrencyType;
        private IRepository<AccountBalance> _repositoryAccountBalance;
        private IRepository<AccountLogin> _repositoryAccountLogin;
        private IRepository<AccountSet> _repositoryAccountSet;
        private IRepository<AccountSetAccounts> _repositoryAccountSetAccounts;

        public AccountsFactory(
            IRepository<Account> repositoryAccount,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AccountBalance> repositoryAccountBalance,
            IRepository<AccountLogin> repositoryAccountLogin,
            IRepository<AccountSet> repositoryAccountSet,
            IRepository<AccountSetAccounts> repositoryAccountSetAccounts,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAccountBalance = repositoryAccountBalance;
            this._repositoryAccountLogin = repositoryAccountLogin;
            this._repositoryAccountSet = repositoryAccountSet;
            this._repositoryAccountSetAccounts = repositoryAccountSetAccounts;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Account> CreateAccount(string email, string name)
        {
            await this.Check(name);

            using (this._scopeableFactory.Create())
            {
                var account = new Account { Email = email, Name = name ?? email.Substring(0, email.IndexOf('@')) };
                await this._repositoryAccount.Add(account);

                CurrencyType currencyType = (await this._repositoryCurrencyType.GetBy(v => v.Name == CurrencyType.Reputation));

                await this.CreateAccountBalance(account, currencyType, 1);

                return account;
            }
        }

        private async Task Check(string name)
        {
            IEnumerable<Account> accounts = await this._repositoryAccount.GetFiltered(a => a.Name == name);

            if (accounts.Any())
                throw new InvalidOperationException("name should be unique");
        }

        public async Task<AccountBalance> CreateAccountBalance(Account account, CurrencyType currencyType, decimal value)
        {
            using (this._scopeableFactory.Create())
            {
                var balance = new AccountBalance { AccountId = account.Id, Value = 1, CurrencyTypeId = currencyType.Id };

                await this._repositoryAccountBalance.Add(balance);

                return balance;
            }
        }

        public async Task<AccountLogin> CreateAccountLogin(Guid accountId, string loginProvider, string providerKey)
        {
            using (this._scopeableFactory.Create())
            {
                AccountLogin accountLogin = new AccountLogin
                {
                    AccountId = accountId,
                    LoginProvider = loginProvider,
                    ProviderKey = providerKey
                };

                await this._repositoryAccountLogin.Add(accountLogin);

                return accountLogin;
            }
        }

        public async Task<AccountBalance> GetOrCreateBalance(Account account, CurrencyType currencyType)
        {
            var accountId = account.Id;
            var currencyTypeId = currencyType.Id;

            var balance = await this._repositoryAccountBalance.GetBy(b => b.AccountId == accountId && b.CurrencyTypeId == currencyTypeId);
            if (balance == null)
            {
                decimal value = currencyType.Name != CurrencyType.Reputation ? 0 : 1;

                await this.CreateAccountBalance(account, currencyType, value);
            }
            return balance;
        }

        public async Task<AccountSet> CreateAccountSet(Account account, IEnumerable<Account> accounts)
        {
            accounts = accounts.Where(a => a != null).Distinct();//normalize

            using (this._scopeableFactory.Create())
            {
                var accountSet = new AccountSet { AccountId = account.Id };
                if (accounts.Any())
                {
                    await this._repositoryAccountSet.Add(accountSet);

                    foreach (var acc in accounts)
                    {
                        if (!acc.IsTransient())
                        {
                            await this._repositoryAccountSetAccounts.Add(new AccountSetAccounts
                            {
                                AccountId = acc.Id,
                                AccountSetId = accountSet.Id
                            });
                        }
                    }
                }
                return accountSet;
            }
        }
    }
}
