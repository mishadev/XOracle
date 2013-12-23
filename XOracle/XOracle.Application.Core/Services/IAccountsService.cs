using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IAccountsService
    {
        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);

        Task<GetAccountResponse> GetAccount(GetAccountRequest request);
    }
}
