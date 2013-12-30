using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IAccountingService
    {
        Task<SingUpResponse> SingUp(SingUpRequest request);

        Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request);

        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);
    }
}
