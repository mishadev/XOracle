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
    [RoutePrefix("api/bet")]
    public class BetsController : ApiController
    {
        private UserManager<IdentityAccount> _userManager;
        private IBetsService _betsService;

        public BetsController()
        {
            var account = CloudConfiguration.GetStorageAccount("DataConnectionString");

            this._betsService = new BetsService(
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
                    new AzureScopeableUnitOfWorkFactory()));

            this._userManager = Startup.UserManagerFactory();
        }

        [Route("CreateBet")]
        public async Task CreateBet(BetBindingModel model)
        {
            IUser account = await this._userManager.FindByNameAsync(User.Identity.Name);

            CreateBetResponse response = await this._betsService.CreateBet(
                new CreateBetRequest
            {
                AccountId = Guid.Parse(account.Id),
                EventId = model.EventId,
                BetAmount = model.BetAmount,
                OutcomesType = model.OutcomesType
            });
        }
    }
}
