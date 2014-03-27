using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Application.Tests
{
    [TestClass]
    public class Tests
    {
        private AccountingService _accountingService;
        private EventsService _eventsService;
        private string _emailFormat = "misha_dev{0}@live.com";
        private BetsService _betsService;

        private string UniqueEmail
        {
            get
            {
                return string.Format(this._emailFormat, Guid.NewGuid().GetHashCode());
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());

            var uow = new InmemoryUnitOfWork();

            _accountingService = new AccountingService(
                new Repository<Account>(uow),
                new Repository<CurrencyType>(uow),
                new Repository<AccountBalance>(uow),
                new Repository<AccountLogin>(uow),
                new Repository<AccountSetAccounts>(uow),
                new AccountsFactory(
                    new Repository<Account>(uow),
                    new Repository<CurrencyType>(uow),
                    new Repository<AccountBalance>(uow),
                    new Repository<AccountLogin>(uow),
                    new Repository<AccountSet>(uow),
                    new Repository<AccountSetAccounts>(uow),
                    new ScopeableUnitOfWorkFactory(uow)));

            _betsService = new BetsService(
                new Repository<Bet>(uow),
                new Repository<Account>(uow),
                new Repository<Event>(uow),
                new Repository<OutcomesType>(uow),
                new Repository<CurrencyType>(uow),
                new BetsFactory(
                    new Repository<Bet>(uow),
                    new Repository<EventBetCondition>(uow),
                    new Repository<EventRelationType>(uow),
                    new Repository<CurrencyType>(uow),
                    new Repository<AlgorithmType>(uow),
                    new Repository<AccountSetAccounts>(uow),
                    new Repository<BetRateAlgorithm>(uow),
                    new ScopeableUnitOfWorkFactory(uow))
                );

            _eventsService = new EventsService(
                new Repository<Account>(uow),
                new Repository<AccountSet>(uow),
                new Repository<EventRelationType>(uow),
                new Repository<CurrencyType>(uow),
                new Repository<AlgorithmType>(uow),
                new Repository<Event>(uow),
                new Repository<EventCondition>(uow),
                new EventsFactory(
                    new Repository<Event>(uow),
                    new Repository<EventCondition>(uow),
                    new Repository<BetRateAlgorithm>(uow),
                    new Repository<EventBetCondition>(uow),
                    new ScopeableUnitOfWorkFactory(uow)),
                _accountingService,
                _betsService,
                new ScopeableUnitOfWorkFactory(uow));
        }

        [TestMethod]
        public async Task UniqueEmailTest()
        {
            Assert.AreNotEqual(this.UniqueEmail, this.UniqueEmail);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateAccountCreatesAccountEmailEmpty()
        {
            await _accountingService.CreateAccount(new CreateAccountRequest { Name = string.Empty, Email = this.UniqueEmail });
        }

        [TestMethod]
        public async Task TestCreateAccountCreatesAccountId()
        {
            var email = this.UniqueEmail;
            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = email });
            var response2 = await _accountingService.GetAccount(new GetAccountRequest { AccountId = response1.AccountId });

            Assert.AreEqual(response1.AccountId, response2.AccountId);
        }

        [TestMethod]
        public async Task TestCreateAccountCreatesAccountName()
        {
            var email = this.UniqueEmail;
            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Name = email });
            var response2 = await _accountingService.GetAccount(new GetAccountRequest { AccountId = response1.AccountId });

            Assert.AreEqual(email, response2.Name);
        }

        [TestMethod]
        public async Task TestCreateAccountCreatesAccountEmail()
        {
            var email = this.UniqueEmail;
            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = email });
            var response2 = await _accountingService.GetAccount(new GetAccountRequest { AccountId = response1.AccountId });

            Assert.AreEqual(email, response2.Email);
        }

        [TestMethod]
        public async Task TestCreateAccountCreatesAccount()
        {
            var email = this.UniqueEmail;
            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = email });
            var response2 = await _accountingService.GetAccount(new GetAccountRequest { AccountId = response1.AccountId, DetalizationLevel = DetalizationLevel.First });

            Assert.AreEqual(1, ((GetAccountResponseFirst)response2).Reputation);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateEventError1()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);
            var accountId = Guid.Empty;

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = accountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { accountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateEventError2()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response1.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateEventEmptyArbiterError()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { Guid.NewGuid() },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });
        }

        [TestMethod]
        public async Task TestCreateEvent()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            Assert.AreNotEqual(Guid.Empty, response.EventId);
        }

        [TestMethod]
        public async Task TestCreateGetEvent()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response3 = await _eventsService.GetEvents(new GetEventsRequest { AccountId = response1.AccountId });

            Assert.IsTrue(response3.Events.Any(r => r.EventId == response.EventId));
        }

        [TestMethod]
        public async Task TestCreateGetEventDetails()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response3 = await _eventsService.GetEventDetails(new GetEventRequest { EventId = response.EventId });

            Assert.AreEqual("a a 2015", response3.Title);
            Assert.AreEqual(end, response3.EndDate);
            Assert.AreEqual(start, response3.StartDate);
        }

        [TestMethod]
        public async Task TestCreateBet()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });

            Assert.AreNotEqual(Guid.Empty, response4.BetId);
        }

        [TestMethod]
        public async Task TestCalculateBetRate()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response8 = await _betsService.CalculateBetRate(new CalculateBetRateRequest { BetAmount = 1, AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });

            Assert.IsTrue(DateTime.Now > response8.CreationDate && DateTime.Now.AddSeconds(-10) < response8.CreationDate);
            Assert.AreEqual(0, response8.WinValue);
            Assert.IsTrue(0.9 < (double)response8.Rate);
        }

        [TestMethod]
        public async Task TestCalculateCoupleBetRate()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response4 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response5 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response6 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response7 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var createBetRequest = new CreateBetRequest { EventId = response.EventId };

            var calculateBetRateRequest = new CalculateBetRateRequest { BetAmount = 1, EventId = response.EventId };
            calculateBetRateRequest.AccountId = response1.AccountId;
            calculateBetRateRequest.OutcomesType = OutcomesType.Happen;

            var calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.IsTrue(DateTime.Now > calculateBetRateResponse.CreationDate && DateTime.Now.AddSeconds(-10) < calculateBetRateResponse.CreationDate);
            Assert.AreEqual(0, calculateBetRateResponse.WinValue);
            Assert.IsTrue(0.99 < (double)calculateBetRateResponse.Rate);

            createBetRequest.AccountId = response1.AccountId;
            createBetRequest.OutcomesType = OutcomesType.Happen;
            var createBetResponse = await _betsService.CreateBet(createBetRequest);

            calculateBetRateRequest.AccountId = response3.AccountId;
            calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.AreEqual(0, calculateBetRateResponse.WinValue);
            Assert.IsTrue(0.99 / 2 < (double)calculateBetRateResponse.Rate);

            createBetRequest.AccountId = response3.AccountId;
            createBetResponse = await _betsService.CreateBet(createBetRequest);

            calculateBetRateRequest.AccountId = response4.AccountId;
            calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.AreEqual(0, calculateBetRateResponse.WinValue);
            Assert.IsTrue(0.99 / 3 < (double)calculateBetRateResponse.Rate);

            createBetRequest.AccountId = response4.AccountId;
            createBetResponse = await _betsService.CreateBet(createBetRequest);

            calculateBetRateRequest.AccountId = response5.AccountId;
            calculateBetRateRequest.OutcomesType = OutcomesType.NotHappen;

            calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.AreEqual(3, calculateBetRateResponse.WinValue);
            Assert.IsTrue(0.99 < (double)calculateBetRateResponse.Rate);

            createBetRequest.AccountId = response5.AccountId;
            createBetRequest.OutcomesType = OutcomesType.NotHappen;
            createBetResponse = await _betsService.CreateBet(createBetRequest);

            calculateBetRateRequest.AccountId = response6.AccountId;
            calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.IsTrue(1.5m > calculateBetRateResponse.WinValue && calculateBetRateResponse.WinValue > 1.49m);
            Assert.IsTrue(0.99 / 2 < (double)calculateBetRateResponse.Rate);

            createBetRequest.AccountId = response6.AccountId;
            createBetResponse = await _betsService.CreateBet(createBetRequest);

            calculateBetRateRequest.AccountId = response7.AccountId;
            calculateBetRateRequest.OutcomesType = OutcomesType.Happen;

            calculateBetRateResponse = await _betsService.CalculateBetRate(calculateBetRateRequest);

            Assert.IsTrue(0.5m > calculateBetRateResponse.WinValue && calculateBetRateResponse.WinValue > 0.49m);
            Assert.IsTrue(0.99 / 4 < (double)calculateBetRateResponse.Rate);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestArbiterBetEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response2.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestOneVsMenyWaitingBetEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestOneVsOneWaitingBetEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response6 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsOne,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            var response5 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
            var response7 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response6.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestOneVsMenySameBetEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            var response5 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
        }

        [TestMethod]
        public async Task TestCreateBetOneVsMeny()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            var response4 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            var response5 = await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });

            Assert.AreNotEqual(Guid.Empty, response4.BetId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateBetReputation()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateBetParticipantsArbiterIntersectEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                ParticipantsAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task TestCreateBetParticipantsNotInEx()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response4 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                ParticipantsAccountIds = new[] { response4.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
        }

        [TestMethod]
        public async Task TestCreateBetParticipants()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            CreateEventResponse response = await _eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.OneVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                ParticipantsAccountIds = new[] { response3.AccountId, Guid.Empty },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            });

            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response1.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.Happen });
            await _betsService.CreateBet(
                new CreateBetRequest { AccountId = response3.AccountId, EventId = response.EventId, OutcomesType = OutcomesType.NotHappen });
        }

        [TestMethod]
        public async Task GetBetsByAccount()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            var ev = new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            };

            var ceResponse1 = await _eventsService.CreateEvent(ev);
            var ceResponse2 = await _eventsService.CreateEvent(ev);
            var ceResponse3 = await _eventsService.CreateEvent(ev);
            var ceResponse4 = await _eventsService.CreateEvent(ev);

            var req = new CreateBetRequest { AccountId = response3.AccountId, OutcomesType = OutcomesType.NotHappen };

            req.EventId = ceResponse1.EventId;
            await _betsService.CreateBet(req);

            req.EventId = ceResponse2.EventId;
            await _betsService.CreateBet(req);

            req.EventId = ceResponse3.EventId;
            await _betsService.CreateBet(req);

            req.EventId = ceResponse4.EventId;
            await _betsService.CreateBet(req);

            var response4 = await _betsService.GetBets(new GetBetsRequest { AccountId = req.AccountId });

            Assert.AreEqual(4, response4.Bets.Count());
        }

        [TestMethod]
        public async Task GetBetsByEvent()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response3 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response4 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response5 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response6 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            var ev = new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            };

            var ceResponse1 = await _eventsService.CreateEvent(ev);

            var req = new CreateBetRequest { EventId = ceResponse1.EventId, OutcomesType = OutcomesType.NotHappen };

            req.AccountId = response3.AccountId;
            await _betsService.CreateBet(req);

            req.AccountId = response4.AccountId;
            await _betsService.CreateBet(req);

            req.AccountId = response5.AccountId;
            await _betsService.CreateBet(req);

            req.AccountId = response6.AccountId;
            await _betsService.CreateBet(req);

            var response7 = await _betsService.GetBets(new GetBetsRequest { EventId = req.EventId });

            Assert.AreEqual(4, response7.Bets.Count());
        }

        [TestMethod]
        public async Task GetBetsDetails()
        {
            var start = DateTime.Now;
            var end = new DateTime(2015, 1, 25);
            var close = start.AddDays((end - start).TotalDays / 2);

            var response1 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });
            var response2 = await _accountingService.CreateAccount(new CreateAccountRequest { Email = this.UniqueEmail });

            var ev = new CreateEventRequest
            {
                AccountId = response1.AccountId,
                Title = "a a 2015",
                ImageUri = "localhost",
                StartDate = start,
                EndDate = end,
                CloseDate = close,
                ExpectedEventCondition = "a a a a a 2015",
                EventRelationType = EventRelationType.MenyVsMeny,
                ArbiterAccountIds = new[] { response2.AccountId },
                AlgorithmType = AlgorithmType.Exponential,
                StartRate = 100,
                LocusRage = 1,
                EndRate = 0,
                CurrencyType = CurrencyType.Reputation
            };

            var ceResponse1 = await _eventsService.CreateEvent(ev);

            var req = new CreateBetRequest { EventId = ceResponse1.EventId, OutcomesType = OutcomesType.NotHappen };

            req.AccountId = response1.AccountId;
            var bet = await _betsService.CreateBet(req);

            var response7 = await _betsService.GetBetsDetails(new GetBetRequest { BetId = bet.BetId });

            Assert.AreEqual(1, response7.Value);
            Assert.IsTrue(DateTime.Now.AddSeconds(-10) < response7.CreationDate);
            Assert.AreEqual(CurrencyType.Reputation, response7.CurrencyType);
            Assert.AreEqual(response1.AccountId, response7.AccountId);
            Assert.AreEqual(ceResponse1.EventId, response7.EventId);
        }
    }
}
