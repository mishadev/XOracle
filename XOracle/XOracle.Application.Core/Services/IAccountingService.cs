using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IAccountingService
    {
        Task<SingInResponse> SingIn(SingInRequest request);

        Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request);
    }
}
