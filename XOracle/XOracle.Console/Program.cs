using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Console
{
    class Program
    {
        static IAccountingService _accountingService;
        static IEventsService _eventsService;
        static IBetsService _betsService;

        static Program()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());

            var uow = new InmemoryUnitOfWork();

            _accountingService = new AccountingService(
                new RepositoryFactory(uow),
                new AccountsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow)));

            _eventsService = new EventsService(
                new RepositoryFactory(uow),
                new EventsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow)),
                new ScopeableUnitOfWorkFactory(uow));

            _betsService = new BetsService(
                new RepositoryFactory(uow),
                new BetsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow))
                );
        }

        static void Main(string[] args)
        {
            /*var singIn = SingUpAccout().GetAwaiter().GetResult();

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
            */
            A();

            System.Console.ReadLine();
        }

        static async Task A()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (await f.Create())
            {
                await repo.Add(new EventRelationType { Name = "x3" });
                using (await f.Create())
                {
                    await repo.Add(new EventRelationType { Name = string.Empty });
                }
            }
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

        static async Task<CreateEventResponse> CreateEvent(Guid accountId)
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = accountId,
                Title = "Янукович призедент 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "Янукович займет пост призедента в 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                JudgingAccountIds = new[] { accountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            return response;
        }
    }
}
