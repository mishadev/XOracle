using System;
using System.Threading.Tasks;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Domain.Services;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;

namespace XOracle.Console
{
    class Program
    {
        static IAccountingService _accountingService;

        static Program()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<ITypeAdapter>.SetCurrent(new AutomapperTypeAdapterFactory());

            _accountingService = new AccountingService(
                new AccountsDomainService(
                    new Repository<Account>(new InmemoryUnitOfWork()),
                    new Repository<Reputation>(new InmemoryUnitOfWork())),
                new LoginDomainService());
        }

        static void Main(string[] args)
        {
            var singIn = SingInAccout().GetAwaiter().GetResult();

            System.Console.WriteLine("AccoutnId : " + singIn.AccountId);
            System.Console.WriteLine("Ticket : " + singIn.Ticket);

            var details = GetAccoutDetails(singIn.AccountId).GetAwaiter().GetResult();

            System.Console.WriteLine("AccoutnId : " + details.AccountId);
            System.Console.WriteLine("EMail : " + details.EMail);
            System.Console.WriteLine("Name : " + details.Name);
            System.Console.WriteLine("Reputation : " + details.Reputation);

            System.Console.ReadLine();
        }

        static async Task<SingInResponse> SingInAccout()
        {
            SingInResponse response = await _accountingService.SingIn(new SingInRequest { EMail = "misha_dev@live.com" });

            return response;
        }

        static async Task<GetDetailsAccountResponse> GetAccoutDetails(Guid accountId)
        {
            GetDetailsAccountResponse response = await _accountingService.GetDetailsAccount(new GetDetailsAccountRequest { AccountId = accountId });

            return response;
        }
    }
}
