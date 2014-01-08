using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Web.Front
{
    public class AccountStore : IUserStore<IdentityAccount>
    {
        private IAccountingService _accountingService;

        public AccountStore()
        {
            IUnitOfWork uow = new InmemoryUnitOfWork();

            this._accountingService = new AccountingService(
                new RepositoryFactory(uow),
                new AccountsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow)));
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

            return new IdentityAccount { Id = response.AccountId.ToString(), UserName = response.Name };
        }

        public async Task<IdentityAccount> FindByNameAsync(string userName)
        {
            var response = await _accountingService.GetAccount(new GetAccountRequest { Name = userName });

            return new IdentityAccount { Id = response.AccountId.ToString(), UserName = response.Name };
        }

        public Task UpdateAsync(IdentityAccount user)
        {
            throw new InvalidOperationException("UpdateAsync is not valid");
        }

        public void Dispose()
        { }
    }
}