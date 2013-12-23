using System.Threading.Tasks;

namespace XOracle.Domain.Core.Services
{
    public interface IEventDomainService
    {
        Task<EventCreateResponse> CreateEvent(EventCreateRequest request);
    }
}
