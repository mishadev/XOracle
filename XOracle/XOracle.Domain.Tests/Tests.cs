using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

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
            var service = new EventService();

            EventCreateResponse response = await service.CreateEvent(new EventCreateRequest());

            Assert.AreNotEqual(default(int), response.Id);
        }
    }
}
