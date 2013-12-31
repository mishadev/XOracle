using System.Threading.Tasks;
using XOracle.Application.Core;
using XOracle.Data.Core;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Application
{
    public class EventsService : IEventsService
    {
        private IRepository<Account> _accountRepository;
        private IRepository<Event> _eventRepository;
        private IRepository<EventBetCondition> _eventBetConditionRepository;
        private IRepository<EventBetRateAlgorithm> _eventBetRateAlgorithm;

        private IFactory<IScopeable<IUnitOfWork>> _scopeableFactory;

        public EventsService(
            IRepository<Account> accountRepository,
            IRepository<Event> eventRepository,
            IRepository<EventBetCondition> eventBetConditionRepository,
            IRepository<EventBetRateAlgorithm> eventBetRateAlgorithm,
            IFactory<IScopeable<IUnitOfWork>> scopeableFactory)
        {
            this._accountRepository = accountRepository;
            this._eventRepository = eventRepository;
            this._eventBetConditionRepository = eventBetConditionRepository;
            this._eventBetRateAlgorithm = eventBetRateAlgorithm;

            this._scopeableFactory = scopeableFactory;
        }

        public async Task<CreateEventResponse> CreateEvent(CreateEventRequest request)
        {


            return new CreateEventResponse();
        }

        public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
        {

            return new GetEventsResponse();
        }
    }
}
