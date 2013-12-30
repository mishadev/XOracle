using System;
using System.Threading.Tasks;

namespace XOracle.Console
{
    using XOracle.Application;
    using XOracle.Application.Core;
    using XOracle.Data;
    using XOracle.Data.Core;
    using XOracle.Domain.Core;
    using XOracle.Infrastructure;
    using XOracle.Infrastructure.Core;
    using XOracle.Infrastructure.Utils;

    class Program
    {
        static IAccountingService _accountingService;

        static Program()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());

            var uow = new InmemoryUnitOfWork();
            _accountingService = new AccountingService(
                new Repository<Account>(uow),
                new Repository<AccountBalance>(uow),
                new Repository<ValueType>(uow),
                new ScopeableUnitOfWorkFactory(uow));
        }

        static void Main(string[] args)
        {
            var singIn = SingUpAccout().GetAwaiter().GetResult();

            System.Console.WriteLine("AccoutnId : " + singIn.AccountId);
            System.Console.WriteLine("Ticket : " + singIn.Ticket);

            var details = GetAccoutDetails(singIn.AccountId).GetAwaiter().GetResult();

            System.Console.WriteLine("AccoutnId : " + details.AccountId);
            System.Console.WriteLine("EMail : " + details.Email);
            System.Console.WriteLine("Name : " + details.Name);
            System.Console.WriteLine("Reputation : " + details.Reputation);

            System.Console.ReadLine();
        }

        static async Task<SingUpResponse> SingUpAccout()
        {
            SingUpResponse response = await _accountingService.SingUp(new SingUpRequest { Email = "misha_dev@live.com" });

            return response;
        }

        static async Task<GetDetailsAccountResponse> GetAccoutDetails(Guid accountId)
        {
            GetDetailsAccountResponse response = await _accountingService.GetDetailsAccount(new GetDetailsAccountRequest { AccountId = accountId });

            return response;
        }
    }
}
