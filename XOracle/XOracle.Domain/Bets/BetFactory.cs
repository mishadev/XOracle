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
    }

    public class BetsFactory : IBetsFactory
    {
        private IRepositoryFactory _repositorise;
        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        public BetsFactory(
            IRepositoryFactory repositorise,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._repositorise = repositorise;
            this._scopeableFactory = scopeableFactory;
        }

        public async Task<Bet> CreateBet(Account account, Event @event, OutcomesType outcomesType, decimal value)
        {
            using (this._scopeableFactory.Create())
            {
                EventBetCondition conditions = await this._repositorise.Get<EventBetCondition>().Get(@event.EventBetConditionId);
                CurrencyType currencyType = await this._repositorise.Get<CurrencyType>().Get(conditions.CurrencyTypeId);

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

                await this._repositorise.Get<Bet>().Add(bet);

                return bet;
            }
        }

        private async Task Checks(Event @event, Account account, OutcomesType targetOutcomesType, EventBetCondition conditions, CurrencyType currencyType)
        {
            var ownerAccountId = @event.AccountId;

            if (conditions.CloseDate < DateTime.Now)
                throw new InvalidOperationException("bet alrady closed");

            IEnumerable<AccountSetAccounts> judgingAccounts =
                await this._repositorise.Get<AccountSetAccounts>().GetFiltered(a => a.AccountSetId == @event.JudgingAccountSetId);

            if (judgingAccounts.Any(a => a.AccountId == account.Id))
                throw new InvalidOperationException("judges contains this account");

            if (@event.ParticipantsAccountSetId != default(Guid))
            {
                IEnumerable<AccountSetAccounts> participantsAccounts =
                    await this._repositorise.Get<AccountSetAccounts>().GetFiltered(a => a.AccountSetId == @event.ParticipantsAccountSetId);

                if (!participantsAccounts.Any(setitem => setitem.AccountId == account.Id) && account.Id != ownerAccountId)
                    throw new InvalidOperationException("Participants account has not you accountId");
            }

            EventRelationType relationType = await this._repositorise.Get<EventRelationType>().Get(@event.EventRelationTypeId);
            Bet bet = await this._repositorise.Get<Bet>().GetBy(b => b.EventId == @event.Id && b.AccountId == account.Id);
            if (currencyType.Name == CurrencyType.Reputation && bet != null)
                throw new InvalidOperationException("you already bet you reputation");

            if (relationType.Name != EventRelationType.MenyVsMeny)
            {
                Bet ownrbet = await this._repositorise.Get<Bet>().GetBy(b => b.EventId == @event.Id && b.AccountId == ownerAccountId);
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
                    IEnumerable<Bet> bets = await this._repositorise.Get<Bet>().GetFiltered(b => b.EventId == @event.Id);
                    if (bets.Count() > 1)
                        throw new InvalidOperationException("alrady bet on all outcomes in One vs One relations");
                }
            }
        }

        public async Task<BetRate> CalculateBetRate(Account account, Event @event, OutcomesType outcomesType, decimal value)
        {
            using (var scope = this._scopeableFactory.Create())
            {
                var bet = await CreateBet(account, @event, outcomesType, value);
                var conditions = await this._repositorise.Get<EventBetCondition>().Get(@event.EventBetConditionId);
                var bets = await this._repositorise.Get<Bet>().GetFiltered(b => b.EventId == @event.Id);
                var algorithm = await this._repositorise.Get<BetRateAlgorithm>().Get(conditions.EventBetRateAlgorithmId);

                DateTime date = DateTime.Now;
                ICalculator<double, DateTime> calculator = await GetBetRateCalculator(algorithm, @event);

                IEnumerable<Bet> samebets = bets.Where(b => b.OutcomesTypeId == bet.OutcomesTypeId);
                IEnumerable<Bet> differentbets = bets.Where(b => b.OutcomesTypeId != bet.OutcomesTypeId);

                decimal samebetssum = samebets.Sum(b => b.Value);
                decimal differentbetssum = differentbets.Sum(b => b.Value);

                decimal betRate = GetRate(bet, samebetssum, calculator, date);

                decimal totalBetRate = samebets.Sum(b => GetRate(b, samebetssum, calculator, date));

                decimal possibleWinValue = differentbetssum / totalBetRate * betRate;

                await scope.Rollback();
                return new BetRate { CreationDate = date, Rate = betRate, PossibleWinValue = possibleWinValue };
            }
        }

        public decimal GetRate(Bet bet, decimal samebetssum, ICalculator<double, DateTime> calculator, DateTime date)
        {
            return this.GetRawRate(bet, calculator, date) / samebetssum;
        }

        public decimal GetRawRate(Bet bet, ICalculator<double, DateTime> calculator, DateTime date)
        {
            var rate = calculator.Calculate(date);

            return bet.Value * (decimal)rate;
        }

        public async Task<ICalculator<double, DateTime>> GetBetRateCalculator(BetRateAlgorithm betRateAlgorithm, Event @event)
        {
            var factory = new BetRateCalculatorFactory(betRateAlgorithm.StartRate, betRateAlgorithm.EndRate, betRateAlgorithm.LocusRage, @event.StartDate, @event.EndDate);
            AlgorithmType algorithmType = await this._repositorise.Get<AlgorithmType>().Get(betRateAlgorithm.AlgorithmTypeId);

            return factory.Create(algorithmType.Name);
        }
    }
}
