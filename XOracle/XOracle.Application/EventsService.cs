using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain;
using XOracle.Infrastructure.Core;

namespace XOracle.Application
{
    public class EventsService : IEventsService
    {
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;
        private IEventsFactory _eventsFactory;

        private IRepository<Account> _repositoryAccount;
        private IRepository<AccountSet> _repositoryAccountSet;
        private IRepository<EventRelationType> _repositoryEventRelationType;
        private IRepository<CurrencyType> _repositoryCurrencyType;
        private IRepository<AlgorithmType> _repositoryAlgorithmType;
        private IRepository<Event> _repositoryEvent;
        private IRepository<EventCondition> _repositoryEventCondition;

        private IAccountingService _accountingService;
        private IBetsService _betsService;

        public EventsService(
            IRepository<Account> repositoryAccount,
            IRepository<AccountSet> repositoryAccountSet,
            IRepository<EventRelationType> repositoryEventRelationType,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AlgorithmType> repositoryAlgorithmType,
            IRepository<Event> repositoryEvent,
            IRepository<EventCondition> repositoryEventCondition,
            IEventsFactory eventsFactory,
            IAccountingService accountingService,
            IBetsService betsService,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryAccountSet = repositoryAccountSet;
            this._repositoryEventRelationType = repositoryEventRelationType;
            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAlgorithmType = repositoryAlgorithmType;
            this._repositoryEvent = repositoryEvent;
            this._repositoryEventCondition = repositoryEventCondition;

            this._eventsFactory = eventsFactory;

            this._accountingService = accountingService;
            this._betsService = betsService;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {
            GetAccountResponse accountResponse = await this._accountingService.GetAccount(new GetAccountRequest { AccountId = request.AccountId });

            EventRelationType relatonType = await this._repositoryEventRelationType.GetBy(t => t.Name == request.EventRelationType);
            CurrencyType currencyType = await this._repositoryCurrencyType.GetBy(t => t.Name == request.CurrencyType);
            AlgorithmType algorithmType = await this._repositoryAlgorithmType.GetBy(t => t.Name == request.AlgorithmType);

            await Check(accountResponse.AccountId, request);

            using (this._scopeableFactory.Create())
            {
                var arbiterAccountSetResponse = await this._accountingService.CreateAccountsSet(
                    new CreateAccountsSetRequest
                    {
                        CreatorAccountId = accountResponse.AccountId,
                        AccountIds = request.ArbiterAccountIds
                    });
                var arbiterAccountSet = await this._repositoryAccountSet.Get(arbiterAccountSetResponse.Id);

                var participantsAccountSetResponse = await this._accountingService.CreateAccountsSet(
                    new CreateAccountsSetRequest
                    {
                        CreatorAccountId = accountResponse.AccountId,
                        AccountIds = request.ParticipantsAccountIds
                    });
                var participantsAccountSet = await this._repositoryAccountSet.Get(participantsAccountSetResponse.Id);

                var account = await this._repositoryAccount.Get(accountResponse.AccountId);

                var eventCondition = await this._eventsFactory.CreateEventCondition(account, request.ExpectedEventCondition);

                var betRateAlgorithm = await this._eventsFactory.CreateEventBetRateAlgorithm(account, algorithmType, request.StartRate, request.EndRate, request.LocusRage);
                var betCondition = await this._eventsFactory.CreateEventBetCondition(account, currencyType, betRateAlgorithm, request.CloseDate);

                var @event = await this._eventsFactory.CreateEvent(
                    account,
                    arbiterAccountSet,
                    participantsAccountSet,
                    eventCondition,
                    relatonType,
                    betCondition,
                    request.Title,
                    request.StartDate,
                    request.EndDate);

                return new CreateEventResponse { EventId = @event.Id };
            }
        }

        private Task Check(Guid accountId, CreateEventRequest request)
        {
            if (request.ArbiterAccountIds.Any(id => id == accountId))
                throw new InvalidOperationException("Arbiter account has you accountId");

            if (request.ArbiterAccountIds.Intersect(request.ParticipantsAccountIds).Any())
                throw new InvalidOperationException("Arbiter accounts should not intersect with participants account");

            return Task.FromResult(true);
        }

        public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
        {
            var accountId = request.AccountId;
            IEnumerable<Event> events = await this._repositoryEvent.GetFiltered(e => e.AccountId == accountId);

            var eventDetails = new List<GetEventResponse>(events.Count());
            foreach (var ev in events)
                eventDetails.Add(await GetDetails(ev, accountId, request.DetalizationLevel));

            return new GetEventsResponse
            {
                Events = eventDetails
            };
        }

        public async Task<GetEventResponse> GetEventDetails(GetEventRequest request)
        {
            Event @event = await this._repositoryEvent.Get(request.EventId);

            return await GetDetails(@event, request.AccountId, request.DetalizationLevel);
        }

        private async Task<GetEventResponse> GetDetails(Event @event, Guid accountId, DetalizationLevel level)
        {
            switch (level)
            {
                case DetalizationLevel.First:
                    return await FillFirstDetalization(@event);
                case DetalizationLevel.Second:
                    return await FillSecondDetalization(@event);
                case DetalizationLevel.Full:
                    return await FillFullDetalization(@event, accountId);
                default:
                    return FillBaseDetalization(@event);
            }
        }

        private GetEventResponse FillBaseDetalization(Event @event, GetEventResponse model = null)
        {
            model = model ?? new GetEventResponse();

            model.EventId = @event.Id;
            model.Title = @event.Title;
            model.StartDate = @event.StartDate;
            model.EndDate = @event.EndDate;
            model.AccountId = @event.AccountId;
            model.ImageId = @event.ImageId;
            model.EventRelationTypeId = @event.EventRelationTypeId;
            model.ParticipantsAccountSetId = @event.ParticipantsAccountSetId;
            model.ArbiterAccountSetId = @event.ArbiterAccountSetId;
            model.ExpectedEventConditionId = @event.ExpectedEventConditionId;
            model.RealEventConditionId = @event.RealEventConditionId;
            model.EventBetConditionId = @event.EventBetConditionId;

            return model;
        }

        private async Task<GetEventResponseFirst> FillFirstDetalization(Event @event, GetEventResponseFirst model = null)
        {
            model = model ?? new GetEventResponseFirst();

            FillBaseDetalization(@event, model);

            var expectedCondition = await this._repositoryEventCondition.Get(model.ExpectedEventConditionId);
            var realCondition = await this._repositoryEventCondition.Get(model.RealEventConditionId);

            model.ExpectedEventCondition = expectedCondition != null ? expectedCondition.Description : string.Empty;
            model.RealEventCondition = realCondition != null ? realCondition.Description : string.Empty;

            return model;
        }

        private async Task<GetEventResponseSecond> FillSecondDetalization(Event @event, GetEventResponseSecond model = null)
        {
            model = model ?? new GetEventResponseSecond();

            await FillFirstDetalization(@event, model);

            var arbiterSetid = model.ArbiterAccountSetId;
            if (arbiterSetid != default(Guid))
            {
                var arbiterAccountSet = await this._accountingService.GetAccountsSet(
                    new GetAccountsSetRequest
                    {
                        AccountSetId = arbiterSetid,
                        DetalizationLevel = DetalizationLevel.First
                    });
                model.ArbiterAccounts = ((GetAccountsSetResponseFirst)arbiterAccountSet).Accounts;
            }

            var participantsSetid = model.ParticipantsAccountSetId;
            if (participantsSetid != default(Guid))
            {
                var participantsSet = await this._accountingService.GetAccountsSet(
                    new GetAccountsSetRequest
                    {
                        AccountSetId = participantsSetid,
                        DetalizationLevel = DetalizationLevel.First
                    });
                model.ParticipantsAccounts = ((GetAccountsSetResponseFirst)participantsSet).Accounts;
            }

            return model;
        }

        private async Task<GetEventResponseFull> FillFullDetalization(Event @event, Guid accountId, GetEventResponseFull model = null)
        {
            model = model ?? new GetEventResponseFull();

            await FillSecondDetalization(@event, model);

            model.BetConditions = await this._betsService.GetBetConditions(
                new GetBetConditionsRequest
                {
                    BetConditionId = @event.EventBetConditionId
                });

            model.HappenBetRate = await this._betsService.CalculateBetRate(
                new CalculateBetRateRequest
                {
                    AccountId = accountId,
                    EventId = @event.Id,
                    OutcomesType = OutcomesType.Happen,
                    BetAmount = 1
                });

            model.NotHappenBetRate = await this._betsService.CalculateBetRate(
                new CalculateBetRateRequest
                {
                    AccountId = accountId,
                    EventId = @event.Id,
                    OutcomesType = OutcomesType.NotHappen,
                    BetAmount = 1
                });

            return model;
        }
    }
}
