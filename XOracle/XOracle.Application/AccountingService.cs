using System.Linq;
using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Application
{
    public class AccountingService : IAccountingService
    {
        private IRepository<Account> _accountRepository;
        private IRepository<AccountBalance> _accountBalanceRepository;
        private IRepository<CurrencyType> _valueTypeRepository;
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        public AccountingService(
            IRepository<Account> accountRepository,
            IRepository<AccountBalance> accountBalanceRepository,
            IRepository<CurrencyType> valueTypeRepository,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._accountRepository = accountRepository;
            this._accountBalanceRepository = accountBalanceRepository;
            this._valueTypeRepository = valueTypeRepository;
            this._scopeableFactory = scopeableFactory;
        }

        public async Task<SingUpResponse> SingUp(SingUpRequest request)
        {
            var creation = await CreateAccount(new CreateAccountRequest { Email = request.Email });

            return new SingUpResponse { AccountId = creation.AccountId, Ticket = System.Guid.NewGuid() };
        }

        public async Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request)
        {
            var account = await this._accountRepository.Get(request.AccountId);
            var valueType = await this._valueTypeRepository.GetBy(v => v.Name == CurrencyType.ReputationName);
            var balance = await this._accountBalanceRepository.GetBy(b => b.AccountId == account.Id && b.CurrencyTypeId == valueType.Id);
            
            return new GetDetailsAccountResponse { AccountId = account.Id, Email = account.Email, Name = account.Name, Reputation = balance.Value };
        }

        public async Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request)
        {
            var account = await this._accountRepository.GetBy(a => a.Email == request.Email);
            if (account == null)
            {
                using (var scope = await this._scopeableFactory.Create())
                {
                    account = new Account { Email = request.Email, Name = request.Name ?? request.Email };
                    await this._accountRepository.Add(account);

                    var valueType = (await this._valueTypeRepository.GetBy(v => v.Name == CurrencyType.ReputationName));
                    var balance = new AccountBalance { AccountId = account.Id, Value = 1, CurrencyTypeId = valueType.Id };
                    await this._accountBalanceRepository.Add(balance);
                }
            }

            return new CreateAccountResponse { AccountId = account.Id };
        }
    }
}
