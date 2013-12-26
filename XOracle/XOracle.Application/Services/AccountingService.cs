using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;
using XOracle.Infrastructure.Core;

namespace XOracle.Application
{
    public class AccountingService : IAccountingService
    {
        private IAccountsDomainService _accountsService;
        private ILoginDomainService _loginService;

        public AccountingService(IAccountsDomainService accountsService, ILoginDomainService loginService)
        {
            this._accountsService = accountsService;
            this._loginService = loginService;
        }

        public async Task<SingInResponse> SingIn(SingInRequest request)
        {
            GetAccountResponse getAccountResponse = await _accountsService.GetAccount(new GetAccountRequest { EMail = request.EMail });
            LoginRequest loginRequest;
            if (!getAccountResponse.HasAccount)
            {
                CreateAccountResponse createAccountResponse = await _accountsService.CreateAccount(new CreateAccountRequest { EMail = request.EMail });
                loginRequest = new LoginRequest { AccountId = createAccountResponse.AccountId };
            }
            else
            {
                loginRequest = new LoginRequest { AccountId = getAccountResponse.AccountId };
            }

            LoginResponse loginResponse = await this._loginService.Login(loginRequest);

            return new SingInResponse { AccountId = loginResponse.AccountId, Ticket = loginResponse.Ticket };
        }

        public Task<GetDetailsAccountResponse> GetDetailsAccount(GetDetailsAccountRequest request)
        {
            
        }
    }
}
