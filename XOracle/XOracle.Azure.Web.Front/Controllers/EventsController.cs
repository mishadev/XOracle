using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using XOracle.Application;
using XOracle.Application.Core;
using XOracle.Azure.Core.Helpers;
using XOracle.Azure.Web.Front.Models;
using XOracle.Data;
using XOracle.Data.Azure;
using XOracle.Data.Azure.Entities;
using XOracle.Data.Core;
using XOracle.Domain;

namespace XOracle.Azure.Web.Front.Controllers
{
    [Authorize]
    [RoutePrefix("api/event")]
    public class EventsController : ApiController
    {
        private UserManager<IdentityAccount> _userManager;
        private IEventsService _eventsService;

        public EventsController()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");

            this._eventsService = new EventsService(
                new AzureRepository<AzureAccount, Account>(account),
                new AzureRepository<AzureEventRelationType, EventRelationType>(account),
                new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                new AzureRepository<AzureAlgorithmType, AlgorithmType>(account),
                new AzureRepository<AzureEvent, Event>(account),
                new EventsFactory(
                    new AzureRepository<AzureAccountSet, AccountSet>(account),
                    new AzureRepository<AzureEvent, Event>(account),
                    new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                    new AzureRepository<AzureEventCondition, EventCondition>(account),
                    new AzureRepository<AzureBetRateAlgorithm, BetRateAlgorithm>(account),
                    new AzureRepository<AzureEventBetCondition, EventBetCondition>(account),
                    new AzureScopeableUnitOfWorkFactory()),
                new AzureScopeableUnitOfWorkFactory());

            this._userManager = Startup.UserManagerFactory();
        }

        [Route("CreateEvent")]
        public async Task<CreateEventResponse> CreateEvent(EventBindingModels model)
        {
            IUser account = await this._userManager.FindByNameAsync(User.Identity.Name);

            return await this._eventsService.CreateEvent(new CreateEventRequest
            {
                AccountId = Guid.Parse(account.Id),
                AlgorithmType = model.AlgorithmType,
                CloseDate = model.CloseDate,
                CurrencyType = model.CurrencyType,
                EndDate = model.EndDate,
                EndRate = model.EndRate,
                EventRelationType = model.EventRelationType,
                ExpectedEventCondition = model.ExpectedEventCondition,
                ImageUri = model.ImageUri,
                JudgingAccountIds = MatchesAccounts(model.JudgingAccounts).Select(a => Guid.Parse(a.Id)),
                LocusRage = model.LocusRage,
                ParticipantsAccountIds = MatchesAccounts(model.ParticipantsAccounts).Select(a => Guid.Parse(a.Id)),
                StartDate = model.StartDate,
                StartRate = model.StartRate,
                Title = model.Title
            });
        }

        private IEnumerable<IdentityAccount> MatchesAccounts(string names)
        {
            if (string.IsNullOrWhiteSpace(names))
                throw new ArgumentNullException("names");

            var matches = Regex.Matches(names, "@\\S*", RegexOptions.Compiled);

            return matches
                .AsParallel()
                .OfType<string>()
                .Select(this._userManager.FindByName)
                .Where(u => u != null)
                .AsEnumerable();
        }

        [Route("GetEvents")]
        public async Task<GetEventsResponse> GetEvents()
        {
            IUser account = await this._userManager.FindByNameAsync(User.Identity.Name);

            return await this._eventsService.GetEvents(new GetEventsRequest { AccountId = Guid.Parse(account.Id) });
        }

        [Route("EventRelationTypes")]
        public IEnumerable<string> GetEventRelationTypes()
        {
            return new[] { EventRelationType.MenyVsMeny, EventRelationType.OneVsMeny, EventRelationType.OneVsOne };
        }
    }
}
