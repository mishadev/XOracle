using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IBetsService
    {
        Task<CreateBetResponse> CreateBet(CreateBetRequest request);

        Task<GetBetRateResponse> GetBetRate(GetBetRateRequest request);

        Task<GetBetsResponse> GetBets(GetBetsRequest request);
    }
}
