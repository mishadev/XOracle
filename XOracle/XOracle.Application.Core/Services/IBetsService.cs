using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IBetsService
    {
        Task<CreateBetResponse> CreateBet(CreateBetRequest request);

        Task<CalculateBetRateResponse> CalculateBetRate(CalculateBetRateRequest request);

        Task<GetBetsResponse> GetBets(GetBetsRequest request);

        Task<GetBetConditionsResponse> GetBetConditions(GetBetConditionsRequest request);
    }
}
