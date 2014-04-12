using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain
{
    public interface IBetsFactory
    {
        Task<Bet> CreateBet(Account account, Event @event, OutcomesType outcomesType, decimal value);

        Task<BetRate> CalculateBetRate(Account account, Event @event, OutcomesType outcomesType, decimal value);

        Task<byte[]> CalculateBetConditionChartData(BetRateAlgorithm betRateAlgorithm);
    }

    public class BetsFactory : IBetsFactory
    {
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        private IRepository<Bet> _repositoryBet;
        private IRepository<EventBetCondition> _repositoryEventBetCondition;
        private IRepository<EventRelationType> _repositoryEventRelationType;

        private IRepository<CurrencyType> _repositoryCurrencyType;
        private IRepository<AlgorithmType> _repositoryAlgorithmType;

        private IRepository<AccountSetAccounts> _repositoryAccountSetAccounts;
        private IRepository<BetRateAlgorithm> _repositoryBetRateAlgorithm;

        public BetsFactory(
            IRepository<Bet> repositoryBet,
            IRepository<EventBetCondition> repositoryEventBetCondition,
            IRepository<EventRelationType> repositoryEventRelationType,
            IRepository<CurrencyType> repositoryCurrencyType,
            IRepository<AlgorithmType> repositoryAlgorithmType,
            IRepository<AccountSetAccounts> repositoryAccountSetAccounts,
            IRepository<BetRateAlgorithm> repositoryBetRateAlgorithm,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositoryBet = repositoryBet;
            this._repositoryEventBetCondition = repositoryEventBetCondition;
            this._repositoryEventRelationType = repositoryEventRelationType;

            this._repositoryCurrencyType = repositoryCurrencyType;
            this._repositoryAlgorithmType = repositoryAlgorithmType;

            this._repositoryAccountSetAccounts = repositoryAccountSetAccounts;
            this._repositoryBetRateAlgorithm = repositoryBetRateAlgorithm;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Bet> CreateBet(Account account, Event @event, OutcomesType outcomesType, decimal value)
        {
            using (this._scopeableFactory.Create())
            {
                EventBetCondition conditions = await this._repositoryEventBetCondition.Get(@event.EventBetConditionId);
                CurrencyType currencyType = await this._repositoryCurrencyType.Get(conditions.CurrencyTypeId);

                await Checks(@event, account, outcomesType, conditions, currencyType);

                if (currencyType.Name == CurrencyType.Reputation)
                    value = 1;

                var bet = new Bet
                {
                    AccountId = account.Id,
                    CreationDate = DateTime.Now,
                    OutcomesTypeId = outcomesType.Id,
                    EventId = @event.Id,
                    CurrencyTypeId = conditions.CurrencyTypeId,
                    Value = value
                };

                await this._repositoryBet.Add(bet);

                return bet;
            }
        }

        private async Task Checks(Event @event, Account account, OutcomesType targetOutcomesType, EventBetCondition conditions, CurrencyType currencyType)
        {
            var ownerAccountId = @event.AccountId;

            if (conditions.CloseDate < DateTime.Now)
                throw new InvalidOperationException("bet alrady closed");

            var accountSetId = @event.ArbiterAccountSetId;
            IEnumerable<AccountSetAccounts> arbiterAccounts =
                await this._repositoryAccountSetAccounts.GetFiltered(a => a.AccountSetId == accountSetId);

            if (arbiterAccounts.Any(a => a.AccountId == account.Id))
                throw new InvalidOperationException("you are arbiter of this bet");

            if (@event.ParticipantsAccountSetId != default(Guid))
            {
                accountSetId = @event.ParticipantsAccountSetId;
                IEnumerable<AccountSetAccounts> participantsAccounts =
                    await this._repositoryAccountSetAccounts.GetFiltered(a => a.AccountSetId == accountSetId);

                if (!participantsAccounts.Any(setitem => setitem.AccountId == account.Id) && account.Id != ownerAccountId)
                    throw new InvalidOperationException("Participants account has not you accountId");
            }

            var eventId = @event.Id;
            var accountId = account.Id;

            EventRelationType relationType = await this._repositoryEventRelationType.Get(@event.EventRelationTypeId);
            Bet bet = await this._repositoryBet.GetBy(b => b.EventId == eventId && b.AccountId == accountId);
            if (currencyType.Name == CurrencyType.Reputation && bet != null)
                throw new InvalidOperationException("you already bet you reputation");

            if (relationType.Name != EventRelationType.MenyVsMeny)
            {
                eventId = @event.Id;

                Bet ownrbet = await this._repositoryBet.GetBy(b => b.EventId == eventId && b.AccountId == ownerAccountId);
                if (ownrbet == null)
                {
                    if (ownerAccountId != account.Id)
                        throw new InvalidOperationException("cannot bet before owner in One vs Meny or One vs One relations");
                }
                else
                {
                    if (ownrbet.OutcomesTypeId == targetOutcomesType.Id)
                        throw new InvalidOperationException("cannot bet same as owner in One vs Meny or One vs One relations");
                }

                if (relationType.Name == EventRelationType.OneVsOne)
                {
                    eventId = @event.Id;
                    IEnumerable<Bet> bets = await this._repositoryBet.GetFiltered(b => b.EventId == eventId);
                    if (bets.Count() > 1)
                        throw new InvalidOperationException("alrady bet on all outcomes in One vs One relations");
                }
            }
        }

        public async Task<BetRate> CalculateBetRate(Account account, Event @event, OutcomesType outcomesType, decimal value)
        {
            using (var scope = this._scopeableFactory.Create())
            {
                EventBetCondition conditions = await this._repositoryEventBetCondition.Get(@event.EventBetConditionId);
                CurrencyType currencyType = await this._repositoryCurrencyType.Get(conditions.CurrencyTypeId);

                try
                {
                    await Checks(@event, account, outcomesType, conditions, currencyType);
                }
                catch { return null; }

                Guid eventId = @event.Id;
                var bets = await this._repositoryBet.GetFiltered(b => b.EventId == eventId);
                var algorithm = await this._repositoryBetRateAlgorithm.Get(conditions.EventBetRateAlgorithmId);

                DateTime date = DateTime.Now;
                ICalculator<double, DateTime> calculator = await GetBetRateCalculator(algorithm, @event);

                IEnumerable<Bet> samebets = bets.Where(b => b.OutcomesTypeId == outcomesType.Id);
                IEnumerable<Bet> differentbets = bets.Where(b => b.OutcomesTypeId != outcomesType.Id);

                decimal betRate = GetRate(value, calculator, date);
                decimal totalBetRate = samebets.Sum(b => GetRate(b.Value, calculator, b.CreationDate));

                decimal differentbetssum = differentbets.Sum(b => b.Value);
                decimal winRate = betRate / (totalBetRate + betRate);
                decimal winValue = differentbetssum * winRate;

                return new BetRate { CreationDate = date, Rate = betRate, WinRate = winRate, WinValue = winValue };
            }
        }

        public decimal GetRate(decimal value, ICalculator<double, DateTime> calculator, DateTime date)
        {
            var rate = calculator.Calculate(date);

            return value * (decimal)rate;
        }

        private async Task<BetRateCalculatorDateTime> GetBetRateCalculator(BetRateAlgorithm betRateAlgorithm, Event @event)
        {
            var factory = new BetRateCalculatorFactory(betRateAlgorithm.LocusRage);
            AlgorithmType algorithmType = await this._repositoryAlgorithmType.Get(betRateAlgorithm.AlgorithmTypeId);

            return factory.CreateDateTime(algorithmType.Name, @event.StartDate, @event.EndDate);
        }

        private async Task<BetRateCalculator> GetBetRateCalculator(BetRateAlgorithm betRateAlgorithm)
        {
            var factory = new BetRateCalculatorFactory(betRateAlgorithm.LocusRage);
            AlgorithmType algorithmType = await this._repositoryAlgorithmType.Get(betRateAlgorithm.AlgorithmTypeId);

            return factory.Create(algorithmType.Name);
        }

        public async Task<byte[]> CalculateBetConditionChartData(BetRateAlgorithm betRateAlgorithm)
        {
            ICalculator<double, double> calculator = await this.GetBetRateCalculator(betRateAlgorithm);

            byte[] data = Enumerable.Range(0, 100)
                .Select(p => (byte)(calculator.Calculate(p / 100.0) * byte.MaxValue))
                .ToArray();

            return data;
        }
    }
}
