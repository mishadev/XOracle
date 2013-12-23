using System.Threading.Tasks;

namespace XOracle.Domain
{
    public class EventService
    {
        public async Task<EventCreateResponse> CreateEvent(EventCreateRequest request)
        {
            return new EventCreateResponse { Id = 1 };
        }
    }
}
