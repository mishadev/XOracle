using System.Collections.Generic;
using System.Web.Http;
using XOracle.Domain;

namespace XOracle.Azure.Web.Front.Controllers
{
    [Authorize]
    [RoutePrefix("api/common")]
    public class CommonController : ApiController
    {
        [Route("CurrencyTypes")]
        public IEnumerable<string> GetCurrencyTypes()
        {
            return new[] { CurrencyType.Reputation };
        }

        [Route("AlgorithmTypes")]
        public IEnumerable<string> GetAlgorithmTypes()
        {
            return new[] { AlgorithmType.Exponential, AlgorithmType.Linear };
        }
    }
}
