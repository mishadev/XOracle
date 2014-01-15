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
        private IRepository<EventRelationType> _repositoryEventRelationType;
        private IRepository<CurrencyType> _repositoryCurrencyType;
        private IRepository<AlgorithmType> _repositoryAlgorithmType;
        private IRepository<Event> _repositoryEvent;
        public EventsService(
            IRepository<Account> repositoryAccount,
            IRepository<EventRelationType> repositoryEventRelationType,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AlgorithmType> repositoryAlgorithmType,
            IRepository<Event> repositoryEvent,
            IEventsFactory eventsFactory,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryEventRelationType = repositoryEventRelationType;
            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAlgorithmType = repositoryAlgorithmType;
            this._repositoryEvent = repositoryEvent;

            this._eventsFactory = eventsFactory;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {
            Account account = await this._repositoryAccount.Get(request.AccountId);
            EventRelationType relatonType = await this._repositoryEventRelationType.GetBy(t => t.Name == request.EventRelationType);
            CurrencyType currencyType = await this._repositoryCurrencyType.GetBy(t => t.Name == request.CurrencyType);
            AlgorithmType algorithmType = await this._repositoryAlgorithmType.GetBy(t => t.Name == request.AlgorithmType);

            await Check(account, request);

            using (this._scopeableFactory.Create())
            {
                var judgingAccountSet = await this._eventsFactory.CreateAccountSet(
                    account,
                    request.JudgingAccountIds.Select(id => this._repositoryAccount.Get(id).GetAwaiter().GetResult()));

                var participantsAccountSet = await this._eventsFactory.CreateAccountSet(
                    account,
                    request.ParticipantsAccountIds.Select(id => this._repositoryAccount.Get(id).GetAwaiter().GetResult()));

                var eventCondition = await this._eventsFactory.CreateEventCondition(account, request.ExpectedEventCondition);

                var betRateAlgorithm = await this._eventsFactory.CreateEventBetRateAlgorithm(account, algorithmType, request.StartRate, request.EndRate, request.LocusRage);
                var betCondition = await this._eventsFactory.CreateEventBetCondition(account, currencyType, betRateAlgorithm, request.CloseDate);

                var @event = await this._eventsFactory.CreateEvent(
                    account,
                    judgingAccountSet,
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

        private async Task Check(Account account, CreateEventRequest request)
        {
            if (request.JudgingAccountIds.Any(id => id == account.Id))
                throw new InvalidOperationException("Judging account has you accountId");

            if (request.JudgingAccountIds.Intersect(request.ParticipantsAccountIds).Any())
                throw new InvalidOperationException("Judging accounts should not intersect with participants account");
        }

        public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
        {
            var accountId = request.AccountId;
            IEnumerable<Event> events = await this._repositoryEvent.GetFiltered(e => e.AccountId == accountId);

            return new GetEventsResponse { EventIds = events.Select(e => e.Id) };
        }

        public async Task<GetEventDetailsResponse> GetEventDetails(GetEventDetailsRequest request)
        {
            Event @event = await this._repositoryEvent.Get(request.EventId);

            return new GetEventDetailsResponse
            {
                Title = @event.Title,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate
            };
        }
    }
}
