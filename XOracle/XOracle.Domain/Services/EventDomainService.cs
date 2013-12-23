using System.Threading.Tasks;
using XOracle.Domain.Core;
using XOracle.Domain.Core.Services;

namespace XOracle.Domain.Services
{
    public class EventDomainService : IEventDomainService
    {
        public async Task<EventCreateResponse> CreateEvent(EventCreateRequest request)
        {
            return new EventCreateResponse { Id = 1 };
        }
    }
}
