using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IBetsService : IAppService
    {
        Task<CreateBetResponse> CreateBet(CreateBetRequest request);

        Task<CalculateBetRateResponse> CalculateBetRate(CalculateBetRateRequest request);

        Task<GetBetsResponse> GetBets(GetBetsRequest request);
    }
}
