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
        private IRepositoryFactory _repositories;

        public IRepositoryFactory Repositories
        {
            get { return _repositories; }
        }

        public BetsService(
            IRepositoryFactory repositories,
            IBetsFactory betsFactory)
        {
            this._repositories = repositories;

            this._betsFactory = betsFactory;
        }

        public async Task<CreateBetResponse> CreateBet(CreateBetRequest request)
        {
            Account account = await this._repositories.Get<Account>().Get(request.AccountId);
            Event @event = await this._repositories.Get<Event>().Get(request.EventId);
            OutcomesType outcomesType = await this._repositories.Get<OutcomesType>().GetBy(v => v.Name == request.OutcomesType);
            
            var bet = await this._betsFactory.CreateBet(account, @event, outcomesType, request.BetAmount);

            return new CreateBetResponse { BetId = bet.Id };
        }

        public async Task<CalculateBetRateResponse> CalculateBetRate(CalculateBetRateRequest request)
        {
            Account account = await this._repositories.Get<Account>().Get(request.AccountId);
            Event @event = await this._repositories.Get<Event>().Get(request.EventId);
            OutcomesType outcomesType = await this._repositories.Get<OutcomesType>().GetBy(v => v.Name == request.OutcomesType);

            BetRate betRate = await this._betsFactory.CalculateBetRate(account, @event, outcomesType, request.BetAmount);

            return new CalculateBetRateResponse { CreationDate = betRate.CreationDate, PossibleWinValue = betRate.PossibleWinValue, Rate = betRate.Rate };
        }

        public async Task<GetBetsResponse> GetBets(GetBetsRequest request)
        {
            IEnumerable<Bet> bets = Enumerable.Empty<Bet>();

            if (request.AccountId.HasValue)
            {
                var accountId = request.AccountId.Value;

                bets = await this._repositories.Get<Bet>().GetFiltered(b => b.AccountId == accountId);
            }

            if (request.EnentId.HasValue)
            {
                var eventId = request.EnentId.Value;

                bets = await this._repositories.Get<Bet>().GetFiltered(b => b.EventId == eventId);
            }

            return new GetBetsResponse { Bets = bets.Select(Convert) };
        }

        public GetBetsDetailsResponse Convert(Bet bet)
        {
            OutcomesType outcomesType = this._repositories.Get<OutcomesType>().Get(bet.OutcomesTypeId).GetAwaiter().GetResult();
            CurrencyType currencyType = this._repositories.Get<CurrencyType>().Get(bet.CurrencyTypeId).GetAwaiter().GetResult();
            
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
            Bet bet = await this._repositories.Get<Bet>().Get(request.BetId);

            return Convert(bet);
        }
    }
}
