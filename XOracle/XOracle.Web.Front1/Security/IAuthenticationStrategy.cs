using System.Security.Principal;

namespace XOracle.Web.Front.Security
{
    public interface IAuthenticationStrategy
    {
        IPrincipal Authenticate(HttpRequestMessage request);
    }
}