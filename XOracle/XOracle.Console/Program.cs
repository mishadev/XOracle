using System;
using System.Threading.Tasks;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Console
{
    class Program
    {
        static IAccountingService _accountingService;
        static IEventsService _eventsService;

        static Program()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());

            var uow = new InmemoryUnitOfWork();

            _accountingService = new AccountingService(
                new Repository<Account>(uow),
                new Repository<AccountBalance>(uow),
                new Repository<CurrencyType>(uow),
                new ScopeableUnitOfWorkFactory(uow));

            _eventsService = new EventsService(
                new Repository<Account>(uow),
                new Repository<Event>(uow),
                new Repository<EventBetCondition>(uow),
                new Repository<EventBetRateAlgorithm>(uow),
                new ScopeableUnitOfWorkFactory(uow));
        }

        static void Main(string[] args)
        {
            var singIn = SingUpAccout().GetAwaiter().GetResult();

            System.Console.WriteLine("===SingUpAccout===");
            System.Console.WriteLine("AccoutnId : " + singIn.AccountId);
            System.Console.WriteLine("Ticket : " + singIn.Ticket);
            System.Console.WriteLine(Environment.NewLine);

            var details = GetAccoutDetails(singIn.AccountId).GetAwaiter().GetResult();

            System.Console.WriteLine("===GetAccoutDetails===");
            System.Console.WriteLine("AccoutnId : " + details.AccountId);
            System.Console.WriteLine("EMail : " + details.Email);
            System.Console.WriteLine("Name : " + details.Name);
            System.Console.WriteLine("Reputation : " + details.Reputation);
            System.Console.WriteLine(Environment.NewLine);

            var @event = CreateEvent(singIn.AccountId).GetAwaiter().GetResult();

            System.Console.WriteLine("===CreateEvent===");
            System.Console.WriteLine("AccoutnId : " + details.AccountId);
            System.Console.WriteLine("EMail : " + details.Email);
            System.Console.WriteLine(Environment.NewLine);
            
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

        static async Task<GetDetailsAccountResponse> CreateEvent(Guid accountId)
        {
            GetDetailsAccountResponse response = await _accountingService.GetDetailsAccount(new GetDetailsAccountRequest { AccountId = accountId });

            return response;
        }
    }
}
