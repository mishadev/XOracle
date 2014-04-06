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
                new AzureRepository<AzureAccountSet, AccountSet>(account),
                new AzureRepository<AzureEventRelationType, EventRelationType>(account),
                new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                new AzureRepository<AzureAlgorithmType, AlgorithmType>(account),
                new AzureRepository<AzureEvent, Event>(account),
                new AzureRepository<AzureEventCondition, EventCondition>(account),
                new EventsFactory(
                    new AzureRepository<AzureEvent, Event>(account),
                    new AzureRepository<AzureEventCondition, EventCondition>(account),
                    new AzureRepository<AzureBetRateAlgorithm, BetRateAlgorithm>(account),
                    new AzureRepository<AzureEventBetCondition, EventBetCondition>(account),
                    new AzureScopeableUnitOfWorkFactory()),
                new AccountingService(
                    new AzureRepository<AzureAccount, Account>(account),
                    new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                    new AzureRepository<AzureAccountBalance, AccountBalance>(account),
                    new AzureRepository<AzureAccountLogin, AccountLogin>(account),
                    new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                    new AccountsFactory(
                        new AzureRepository<AzureAccount, Account>(account),
                        new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                        new AzureRepository<AzureAccountBalance, AccountBalance>(account),
                        new AzureRepository<AzureAccountLogin, AccountLogin>(account),
                        new AzureRepository<AzureAccountSet, AccountSet>(account),
                        new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                        new AzureScopeableUnitOfWorkFactory())),
                new BetsService(
                    new AzureRepository<AzureBet, Bet>(account),
                    new AzureRepository<AzureAccount, Account>(account),
                    new AzureRepository<AzureEvent, Event>(account),
                    new AzureRepository<AzureOutcomesType, OutcomesType>(account),
                    new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                    new AzureRepository<AzureEventBetCondition, EventBetCondition>(account),
                    new AzureRepository<AzureBetRateAlgorithm, BetRateAlgorithm>(account),
                    new BetsFactory(
                        new AzureRepository<AzureBet, Bet>(account),
                        new AzureRepository<AzureEventBetCondition, EventBetCondition>(account),
                        new AzureRepository<AzureEventRelationType, EventRelationType>(account),
                        new AzureRepository<AzureCurrencyType, CurrencyType>(account),
                        new AzureRepository<AzureAlgorithmType, AlgorithmType>(account),
                        new AzureRepository<AzureAccountSetAccounts, AccountSetAccounts>(account),
                        new AzureRepository<AzureBetRateAlgorithm, BetRateAlgorithm>(account),
                        new AzureScopeableUnitOfWorkFactory())),
                new AzureScopeableUnitOfWorkFactory());

            this._userManager = Startup.UserManagerFactory();
        }

        [Route("CreateEvent")]
        public async Task<Guid> CreateEvent(EventBindingModel model)
        {
            IUser account = await this._userManager.FindByIdAsync(User.Identity.GetUserId());

            CreateEventResponse response = await this._eventsService.CreateEvent(new CreateEventRequest
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
                ArbiterAccountIds = (await this.MatchesAccounts(model.ArbiterAccounts)).Select(a => Guid.Parse(a.Id)),
                LocusRage = model.LocusRage,
                ParticipantsAccountIds = (await this.MatchesAccounts(model.ParticipantsAccounts)).Select(a => Guid.Parse(a.Id)),
                StartDate = model.StartDate,
                StartRate = model.StartRate,
                Title = model.Title
            });

            return response.EventId;
        }

        private async Task<IEnumerable<IdentityAccount>> MatchesAccounts(string names)
        {
            var list = new List<IdentityAccount>();

            if (!string.IsNullOrWhiteSpace(names))
            {
                var matches = Regex.Matches(names, "@[^\\s@]*", RegexOptions.Compiled);
                foreach (Match match in matches)
                    list.Add(await this._userManager.FindByNameAsync(match.Value.Substring(1)));
            }

            return list.Where(u => u != null);
        }

        [Route("GetEvents")]
        public async Task<IEnumerable<EventBrieflyViewModel>> GetEvents()
        {
            IUser account = await this._userManager.FindByIdAsync(User.Identity.GetUserId());
            GetEventsResponse response = await this._eventsService.GetEvents(
                new GetEventsRequest
                {
                    AccountId = Guid.Parse(account.Id),
                    DetalizationLevel = DetalizationLevel.Full
                });

            var events = response.Events.Select(e => Convert((GetEventResponseFull)e));
            return events;
        }

        [Route("GetEvents")]
        public async Task<IEnumerable<EventBrieflyViewModel>> GetEvents(string accountId)
        {
            IUser account = await this._userManager.FindByIdAsync(accountId);
            if (account != null)
            {
                GetEventsResponse response = await this._eventsService.GetEvents(
                new GetEventsRequest
                {
                    AccountId = Guid.Parse(account.Id),
                    DetalizationLevel = DetalizationLevel.Full
                });

                var events = response.Events.Select(e => Convert((GetEventResponseFull)e));

                return events;
            }

            return Enumerable.Empty<EventBrieflyViewModel>();
        }

        private EventBrieflyViewModel Convert(GetEventResponseFull @event)
        {
            return new EventBrieflyViewModel
            {
                EventId = @event.EventId,
                Title = @event.Title,
                Condition = @event.ExpectedEventCondition,
                StartDate = @event.StartDate,
                EndDate = @event.EndDate,
                TimeToClose = (int)(DateTime.Now - @event.BetConditions.CloseDate).TotalSeconds,
                ArbiterAccountSet = @event.ArbiterAccounts.Select(Convert),
                HappenBetRate = Convert(@event.HappenBetRate),
                NotHappenBetRate = Convert(@event.NotHappenBetRate)
            };
        }

        private AccountViewModel Convert(GetAccountResponse response)
        {
            return new AccountViewModel
            {
                Id = response.AccountId.ToString(),
                Name = response.Name
            };
        }

        private BetRateViewModel Convert(CalculateBetRateResponse response)
        {
            if (response == null)
                return null;

            return new BetRateViewModel
            {
                Rate = response.Rate,
                WinRate = response.WinRate,
                WinValue = response.WinValue
            };
        }

        [Route("EventRelationTypes")]
        public IEnumerable<string> GetEventRelationTypes()
        {
            return new[] { EventRelationType.MenyVsMeny, EventRelationType.OneVsMeny, EventRelationType.OneVsOne };
        }

        [Route("GetEvent")]
        public async Task<EventBrieflyViewModel> GetEvent(Guid eventId)
        {
            IUser account = await this._userManager.FindByIdAsync(User.Identity.GetUserId());
            GetEventRequest request = new GetEventRequest
            {
                AccountId = Guid.Parse(account.Id),
                EventId = eventId,
                DetalizationLevel = DetalizationLevel.Full
            };

            GetEventResponse @event = await this._eventsService.GetEventDetails(request);

            return Convert((GetEventResponseFull)@event);
        }
    }
}
