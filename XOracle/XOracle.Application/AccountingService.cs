using System.Linq;
using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain.Core;

namespace XOracle.Application
{
    public class AccountingService : IAccountingService
    {
        private IRepository<Account> _accountRepository;
        private IRepository<AccountBalance> _accountBalanceRepository;
        private IRepository<AccountTransaction> _accountTransactionRepository;
        private IRepository<ValueType> _valueTypeRepository;

        public AccountingService(
            IRepository<Account> accountRepository,
            IRepository<AccountBalance> accountBalanceRepository,
            IRepository<AccountTransaction> accountTransactionRepository,
            IRepository<ValueType> valueTypeRepository)
        {
            this._accountRepository = accountRepository;
            this._accountBalanceRepository = accountBalanceRepository;
            this._accountTransactionRepository = accountTransactionRepository;
            this._valueTypeRepository = valueTypeRepository;
        }

        public async Task<SingInResponse> SingIn(SingInRequest request)
        {
            throw new System.NotImplementedException();
        }

        public async Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request)
        {
            var account = await this._accountRepository.Get(request.AccountId);
            var valueType = (await this._valueTypeRepository.GetFiltered(v => v.Name.Equals(ValueType.ReputationName))).First();
            var balance = (await this._accountBalanceRepository.GetFiltered(b => b.AccountId == account.Id && b.ValueTypeId == valueType.Id)).First();
            
            return new GetDetailsAccountResponse { AccountId = account.Id, Email = account.Email, Name = account.Name, Reputation = balance.Value };
        }
    }
}
