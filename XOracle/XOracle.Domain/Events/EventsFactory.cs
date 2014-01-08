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
            AccountSet judgingAccountSet,
            AccountSet participantsAccountSet,
            EventCondition eventCondition,
            EventRelationType eventRelationType,
            EventBetCondition eventBetCondition,
            string title,
            DateTime startDate,
            DateTime endDate);

        Task<AccountSet> CreateAccountSet(Account account, IEnumerable<Account> accounts);

        Task<EventCondition> CreateEventCondition(string description);

        Task<BetRateAlgorithm> CreateEventBetRateAlgorithm(AlgorithmType algorithmType, double startRate, double endRate, double locusRage);

        Task<EventBetCondition> CreateEventBetCondition(CurrencyType currencyType, BetRateAlgorithm betRateAlgorithm, DateTime closeDate);
    }

    public class EventsFactory : IEventsFactory
    {
        private IRepositoryFactory _repositories;
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        public EventsFactory(
            IRepositoryFactory repositories,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositories = repositories;
            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Event> CreateEvent(
            Account account,
            AccountSet judgingAccountSet,
            AccountSet participantsAccountSet,
            EventCondition eventCondition,
            EventRelationType eventRelationType,
            EventBetCondition eventBetCondition,
            string title,
            DateTime startDate,
            DateTime endDate)
        {
            using (await this._scopeableFactory.Create())
            {
                var @event = new Event
                {
                    AccountId = account.Id,
                    Title = title,
                    StartDate = startDate,
                    EndDate = endDate,
                    EventRelationTypeId = eventRelationType.Id,
                    JudgingAccountSetId = judgingAccountSet.Id,
                    ParticipantsAccountSetId = participantsAccountSet.Id,
                    ExpectedEventConditionId = eventCondition.Id,
                    EventBetConditionId = eventBetCondition.Id
                };

                await this._repositories.Get<Event>().Add(@event);

                return @event;
            }
        }

        public async Task<AccountSet> CreateAccountSet(Account account, IEnumerable<Account> accounts)
        {
            accounts = accounts.Where(a => a != null).Distinct();//normalize

            using (await this._scopeableFactory.Create())
            {
                var accountSet = new AccountSet { AccountId = account.Id };
                if (accounts.Any())
                {
                    await this._repositories.Get<AccountSet>().Add(accountSet);

                    foreach (var acc in accounts)
                    {
                        if (!acc.IsTransient())
                        {
                            await this._repositories.Get<AccountSetAccounts>().Add(new AccountSetAccounts
                            {
                                AccountId = acc.Id,
                                AccountSetId = accountSet.Id
                            });
                        }
                    }
                }
                return accountSet;
            }
        }

        public async Task<EventCondition> CreateEventCondition(string description)
        {
            using (await this._scopeableFactory.Create())
            {
                var condition = new EventCondition { Description = description };

                await this._repositories.Get<EventCondition>().Add(condition);

                return condition;
            }
        }

        public async Task<BetRateAlgorithm> CreateEventBetRateAlgorithm(AlgorithmType algorithmType, double startRate, double endRate, double locusRage)
        {
            using (await this._scopeableFactory.Create())
            {
                var betRateAlgorithm = new BetRateAlgorithm
                {
                    AlgorithmTypeId = algorithmType.Id,
                    StartRate = startRate,
                    EndRate = endRate,
                    LocusRage = locusRage
                };

                await this._repositories.Get<BetRateAlgorithm>().Add(betRateAlgorithm);

                return betRateAlgorithm;
            }
        }

        public async Task<EventBetCondition> CreateEventBetCondition(CurrencyType currencyType, BetRateAlgorithm betRateAlgorithm, DateTime closeDate)
        {
            using (await this._scopeableFactory.Create())
            {
                var eventBetCondition = new EventBetCondition
                {
                    CloseDate = closeDate,
                    CurrencyTypeId = currencyType.Id,
                    EventBetRateAlgorithmId = betRateAlgorithm.Id,
                };

                await this._repositories.Get<EventBetCondition>().Add(eventBetCondition);

                return eventBetCondition;
            }
        }
    }
}
