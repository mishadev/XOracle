using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IEventsService : IAppService
    {
        Task<CreateEventResponse> CreateEvent(CreateEventRequest request);

        Task<GetEventsResponse> GetEvents(GetEventsRequest request);

        Task<GetEventDetailsResponse> GetEventDetails(GetEventDetailsRequest request);
    }
}
