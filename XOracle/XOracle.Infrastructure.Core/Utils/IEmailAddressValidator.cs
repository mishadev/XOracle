using System.Threading.Tasks;

namespace XOracle.Infrastructure.Core
{
    public interface IEmailAddressValidator
    {
        Task<bool> IsValid(string emailAddress);
    }
}
