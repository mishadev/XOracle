using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Web.Front.Controllers
{
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

        [HttpPost]
        public async Task<JsonResult<CreateEventResponse>> Post([FromBody] CreateEventRequest request)
        {
            return Json(await this._eventsService.CreateEvent(request));
        }

        [HttpGet]
        public async Task<JsonResult<GetEventsResponse>> Get([FromUri] GetEventsRequest request)
        {
            return Json(await this._eventsService.GetEvents(request));
        }
    }
}
