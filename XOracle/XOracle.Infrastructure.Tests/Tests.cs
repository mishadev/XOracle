using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Infrastructure.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
        }

        private string GetMessage(Exception ex)
        {
            string message = ex.Message;

            if (ex.InnerException != null)
            {
                message += Environment.NewLine + ex.InnerException.Message;
            }

            return message;
        }

        [TestMethod]
        public async Task LoggingCanBeCreated()
        {
            ILogger logger = await Factory<ILogger>.GetInstance();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public async Task LoggingCanCallDebugMethodth()
        {
            ILogger logger = await Factory<ILogger>.GetInstance();

            await logger.Debug(null);
            await logger.Debug(string.Empty);
            await logger.Debug(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallFatalMethodth()
        {
            ILogger logger = await Factory<ILogger>.GetInstance();

            await logger.Fatal(string.Empty);
            await logger.Fatal(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogErrorMethodth()
        {
            ILogger logger = await Factory<ILogger>.GetInstance();

            await logger.LogError(string.Empty);
            await logger.LogError(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogInfoMethodth()
        {
            ILogger logger = await Factory<ILogger>.GetInstance();

            await logger.LogInfo(string.Empty);
            await logger.LogInfo(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task ValidatorCanCallIsValidMethodth()
        {
            IValidator validator = await Factory<IValidator>.GetInstance();

            Assert.IsTrue(validator.IsValid(string.Empty));
        }

        [TestMethod]
        public async Task ValidatorCanCallGetErrorMessagesMethodth()
        {
            IValidator validator = await Factory<IValidator>.GetInstance();

            Assert.AreEqual(Enumerable.Empty<string>().Count(), validator.GetErrorMessages(string.Empty).Count());
        }

        [TestMethod]
        public async Task BinarySerealizerMethodth1()
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            var e1 = new Dictionary<int, long>();
            var bytes = await serializer.ToBinary(e1);
            var e2 = await serializer.FromBinary(bytes);

            Assert.AreEqual(e1.GetType(), e2.GetType());
        }

        [TestMethod]
        public async Task BinarySerealizerMethodth2()
        {
            var serializer = await Factory<IBinarySerializer>.GetInstance();

            var e1 = new Dictionary<int, long> { { 15, 300 } };
            var bytes = await serializer.ToBinary(e1);
            var e2 = (Dictionary<int, long>)await serializer.FromBinary(bytes);

            Assert.AreEqual(e1[15], e2[15]);
        }
    }
}
