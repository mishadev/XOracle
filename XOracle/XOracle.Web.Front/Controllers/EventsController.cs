using System;
using System.Threading.Tasks;
using System.Web.Http;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Web.Front.Controllers
{
    [Authorize]
    [RoutePrefix("api/event")]
    public class EventsController : ApiController
    {
        private IEventsService _eventsService;

        public EventsController()
        {
            var uow = new InmemoryUnitOfWork();

            this._eventsService = new EventsService(
                new RepositoryFactory(uow),
                new EventsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow)),
                new ScopeableUnitOfWorkFactory(uow));
        }

        [Route("CreateEvent")]
        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {
            return await this._eventsService.CreateEvent(request);
        }

        [AllowAnonymous]
        [Route("CreateEvent")]
        public async Task<GetEventsResponse> CreateEvent(string accountId)
        {
            return await this._eventsService.GetEvents(new GetEventsRequest { AccountId = Guid.Parse(accountId) });
        }
    }
}
