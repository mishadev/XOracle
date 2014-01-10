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
        private IRepositoryFactory _repositories;

        public IRepositoryFactory Repositories
        {
            get { return _repositories; }
        }

        public EventsService(
            IRepositoryFactory repositories,
            IEventsFactory eventsFactory,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositories = repositories;

            this._eventsFactory = eventsFactory;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {
            Account account = await this._repositories.Get<Account>().Get(request.AccountId);
            EventRelationType relatonType = await this._repositories.Get<EventRelationType>().GetBy(t => t.Name == request.EventRelationType);
            CurrencyType currencyType = await this._repositories.Get<CurrencyType>().GetBy(t => t.Name == request.CurrencyType);
            AlgorithmType algorithmType = await this._repositories.Get<AlgorithmType>().GetBy(t => t.Name == request.AlgorithmType);

            await Check(account, request);

            using (await this._scopeableFactory.Create())
            {
                var judgingAccountSet = await this._eventsFactory.CreateAccountSet(
                    account,
                    request.JudgingAccountIds.Select(id => this._repositories.Get<Account>().Get(id).GetAwaiter().GetResult()));

                var participantsAccountSet = await this._eventsFactory.CreateAccountSet(
                    account,
                    request.ParticipantsAccountIds.Select(id => this._repositories.Get<Account>().Get(id).GetAwaiter().GetResult()));

                var eventCondition = await this._eventsFactory.CreateEventCondition(request.ExpectedEventCondition);

                var betRateAlgorithm = await this._eventsFactory.CreateEventBetRateAlgorithm(algorithmType, request.StartRate, request.EndRate, request.LocusRage);
                var betCondition = await this._eventsFactory.CreateEventBetCondition(currencyType, betRateAlgorithm, request.CloseDate);

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
            IEnumerable<Event> events = await this._repositories.Get<Event>().GetFiltered(e => e.AccountId == request.AccountId);

            return new GetEventsResponse { EventIds = events.Select(e => e.Id) };
        }

        public async Task<GetEventDetailsResponse> GetEventDetails(GetEventDetailsRequest request)
        {
            Event @event = await this._repositories.Get<Event>().Get(request.EventId);

            return new GetEventDetailsResponse
            {
                Title = @event.Title,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate
            };
        }
    }
}
