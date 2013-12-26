using System;
using System.Threading.Tasks;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;

namespace XOracle.Domain.Services
{
    public class LoginDomainService : ILoginDomainService
    {
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            return new LoginResponse { AccountId = request.AccountId, Ticket = Guid.NewGuid() };
        }
    }
}
