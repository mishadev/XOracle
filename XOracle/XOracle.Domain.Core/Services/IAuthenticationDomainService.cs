using System.Threading.Tasks;

namespace XOracle.Domain.Core.Services
{
    public interface ILoginDomainService
    {
        Task<LoginResponse> Login(LoginRequest request);
    }
}
