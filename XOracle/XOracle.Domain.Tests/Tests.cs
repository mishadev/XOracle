using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using XOracle.Domain.Core;
using XOracle.Domain.Services;

namespace XOracle.Domain.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        { }

        [TestMethod]
        public async Task TryCreateEvent()
        {
            var service = new EventDomainService();

            EventCreateResponse response = await service.CreateEvent(new EventCreateRequest());

            Assert.AreNotEqual(default(int), response.Id);
        }
    }
}
