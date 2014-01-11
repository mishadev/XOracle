using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Application
{
    public class BetsService : IBetsService
    {
        private IBetsFactory _betsFactory;

        private IRepository<Account> _repositoryAccount;
        private IRepository<Event> _repositoryEvent;
        private IRepository<OutcomesType> _repositoryOutcomesType;
        private IRepository<Bet> _repositoryBet;
        private IRepository<CurrencyType> _repositoryCurrencyType;

        public BetsService(
            IRepository<Account> repositoryAccount,
            IRepository<Event> repositoryEvent,
            IRepository<OutcomesType> repositoryOutcomesType,
            IRepository<Bet> repositoryBet,
            IRepository<CurrencyType> repositoryCurrencyType,
            IBetsFactory betsFactory)
        {
            this._repositoryAccount = repositoryAccount;
            this._repositoryEvent = repositoryEvent;
            this._repositoryOutcomesType = repositoryOutcomesType;
            this._repositoryBet = repositoryBet;
            this._repositoryCurrencyType = repositoryCurrencyType;

            this._betsFactory = betsFactory;
        }

        public async Task<CreateBetResponse> CreateBet(CreateBetRequest request)
        {
            Account account = await this._repositoryAccount.Get(request.AccountId);
            Event @event = await this._repositoryEvent.Get(request.EventId);
            OutcomesType outcomesType = await this._repositoryOutcomesType.GetBy(v => v.Name == request.OutcomesType);
            
            var bet = await this._betsFactory.CreateBet(account, @event, outcomesType, request.BetAmount);

            return new CreateBetResponse { BetId = bet.Id };
        }

        public async Task<CalculateBetRateResponse> CalculateBetRate(CalculateBetRateRequest request)
        {
            Account account = await this._repositoryAccount.Get(request.AccountId);
            Event @event = await this._repositoryEvent.Get(request.EventId);
            OutcomesType outcomesType = await this._repositoryOutcomesType.GetBy(v => v.Name == request.OutcomesType);

            BetRate betRate = await this._betsFactory.CalculateBetRate(account, @event, outcomesType, request.BetAmount);

            return new CalculateBetRateResponse { CreationDate = betRate.CreationDate, PossibleWinValue = betRate.PossibleWinValue, Rate = betRate.Rate };
        }

        public async Task<GetBetsResponse> GetBets(GetBetsRequest request)
        {
            IEnumerable<Bet> bets = Enumerable.Empty<Bet>();

            if (request.AccountId.HasValue)
            {
                var accountId = request.AccountId.Value;

                bets = await this._repositoryBet.GetFiltered(b => b.AccountId == accountId);
            }

            if (request.EnentId.HasValue)
            {
                var eventId = request.EnentId.Value;

                bets = await this._repositoryBet.GetFiltered(b => b.EventId == eventId);
            }

            return new GetBetsResponse { Bets = bets.Select(Convert) };
        }

        public GetBetsDetailsResponse Convert(Bet bet)
        {
            OutcomesType outcomesType = this._repositoryOutcomesType.Get(bet.OutcomesTypeId).GetAwaiter().GetResult();
            CurrencyType currencyType = this._repositoryCurrencyType.Get(bet.CurrencyTypeId).GetAwaiter().GetResult();
            
            return new GetBetsDetailsResponse
            {
                AccountId = bet.AccountId,
                EventId = bet.EventId,
                OutcomesType = outcomesType.Name,
                CurrencyType = currencyType.Name,
                CreationDate = bet.CreationDate,
                Value = bet.Value
            };
        }

        public async Task<GetBetsDetailsResponse> GetBetsDetails(GetBetsDetailsRequest request)
        {
            Bet bet = await this._repositoryBet.Get(request.BetId);

            return Convert(bet);
        }
    }
}
