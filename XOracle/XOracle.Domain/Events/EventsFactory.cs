using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain
{
    public interface IEventsFactory
    {
        Task<Event> CreateEvent(
            Account account,
            AccountSet arbiterAccountSet,
            AccountSet participantsAccountSet,
            EventCondition eventCondition,
            EventRelationType eventRelationType,
            EventBetCondition eventBetCondition,
            string title,
            DateTime startDate,
            DateTime endDate);

        Task<EventCondition> CreateEventCondition(Account account, string description);

        Task<BetRateAlgorithm> CreateEventBetRateAlgorithm(Account account, AlgorithmType algorithmType, double startRate, double endRate, double locusRage);

        Task<EventBetCondition> CreateEventBetCondition(Account account, CurrencyType currencyType, BetRateAlgorithm betRateAlgorithm, DateTime closeDate);
    }

    public class EventsFactory : IEventsFactory
    {
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        private IRepository<Event> _repositoryEvent;
        private IRepository<EventCondition> _repositoryEventCondition;
        private IRepository<BetRateAlgorithm> _repositoryBetRateAlgorithm;
        private IRepository<EventBetCondition> _repositoryEventBetCondition;

        public EventsFactory(
            IRepository<Event> repositoryEvent,
            IRepository<EventCondition> repositoryEventCondition,
            IRepository<BetRateAlgorithm> repositoryBetRateAlgorithm,
            IRepository<EventBetCondition> repositoryEventBetCondition,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositoryEvent = repositoryEvent;
            this._repositoryEventCondition = repositoryEventCondition;
            this._repositoryBetRateAlgorithm = repositoryBetRateAlgorithm;
            this._repositoryEventBetCondition = repositoryEventBetCondition;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Event> CreateEvent(
            Account account,
            AccountSet arbiterAccountSet,
            AccountSet participantsAccountSet,
            EventCondition eventCondition,
            EventRelationType eventRelationType,
            EventBetCondition eventBetCondition,
            string title,
            DateTime startDate,
            DateTime endDate)
        {
            using (this._scopeableFactory.Create())
            {
                var @event = new Event
                {
                    AccountId = account.Id,
                    Title = title,
                    StartDate = startDate == default(DateTime) ? DateTime.Now : startDate,
                    EndDate = endDate,
                    EventRelationTypeId = eventRelationType.Id,
                    ArbiterAccountSetId = arbiterAccountSet.Id,
                    ParticipantsAccountSetId = participantsAccountSet.Id,
                    ExpectedEventConditionId = eventCondition.Id,
                    EventBetConditionId = eventBetCondition.Id
                };

                await this._repositoryEvent.Add(@event);

                return @event;
            }
        }

        

        public async Task<EventCondition> CreateEventCondition(Account account, string description)
        {
            using (this._scopeableFactory.Create())
            {
                var condition = new EventCondition
                {
                    AccountId = account.Id,
                    Description = description
                };

                await this._repositoryEventCondition.Add(condition);

                return condition;
            }
        }

        public async Task<BetRateAlgorithm> CreateEventBetRateAlgorithm(Account account, AlgorithmType algorithmType, double startRate, double endRate, double locusRage)
        {
            using (this._scopeableFactory.Create())
            {
                var betRateAlgorithm = new BetRateAlgorithm
                {
                    AccountId = account.Id,
                    AlgorithmTypeId = algorithmType.Id,
                    StartRate = startRate,
                    EndRate = endRate,
                    LocusRage = locusRage
                };

                await this._repositoryBetRateAlgorithm.Add(betRateAlgorithm);

                return betRateAlgorithm;
            }
        }

        public async Task<EventBetCondition> CreateEventBetCondition(Account account, CurrencyType currencyType, BetRateAlgorithm betRateAlgorithm, DateTime closeDate)
        {
            using (this._scopeableFactory.Create())
            {
                var eventBetCondition = new EventBetCondition
                {
                    AccountId = account.Id,
                    CloseDate = closeDate,
                    CurrencyTypeId = currencyType.Id,
                    EventBetRateAlgorithmId = betRateAlgorithm.Id,
                };

                await this._repositoryEventBetCondition.Add(eventBetCondition);

                return eventBetCondition;
            }
        }
    }
}
