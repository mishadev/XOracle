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
    public class AccountingController : ApiController
    {
        private IAccountingService _accountingService;

        public AccountingController()
        {
            var uow = new InmemoryUnitOfWork();

            this._accountingService = new AccountingService(
                new RepositoryFactory(uow),
                new AccountsFactory(
                    new RepositoryFactory(uow),
                    new ScopeableUnitOfWorkFactory(uow)));
        }

        [HttpPost]
        public async Task<JsonResult<SingUpResponse>> SingUp([FromBody] SingUpRequest request)
        {
            return Json(await this._accountingService.SingUp(request));
        }

        [HttpGet]
        public async Task<JsonResult<GetAccountResponse>> Get([FromUri] GetAccountRequest request)
        {
            return Json(await this._accountingService.GetAccount(request));
        }
    }
}
