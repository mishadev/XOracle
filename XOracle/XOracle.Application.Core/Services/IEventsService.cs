﻿using System.Threading.Tasks;

namespace XOracle.Application.Core
{
    public interface IEventsService
    {
        Task<CreateEventResponse> CreateEvent(CreateEventRequest request);

        Task<GetEventsResponse> GetEvents(GetEventsRequest request);

        Task<GetEventDetailsResponse> GetEventDetails(GetEventDetailsRequest request);
    }
}
