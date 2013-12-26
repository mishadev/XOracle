using System.Threading.Tasks;

namespace XOracle.Domain.Core.Services
{
    public interface IAccountsDomainService
    {
        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);

        Task<GetAccountResponse> GetAccount(GetAccountRequest request);

        Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request);
    }
}
