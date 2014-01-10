using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IAccountingService : IAppService
    {
        Task<GetAccountResponse> GetAccount(GetAccountRequest request);

        Task<CreateAccountResponse> CreateAccount(CreateAccountRequest request);

        Task<DeleteAccountResponse> DeleteAccount(DeleteAccountRequest request);

        Task<GetAccountLoginResponse> GetAccountLogin(GetAccountLoginRequest request);

        Task<GetAccountLoginsResponse> GetAccountLogins(GetAccountLoginsRequest request);

        Task<CreateAccountLoginResponse> CreateAccountLogin(CreateAccountLoginRequest request);
    }
}
