using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            LoggerFactory.SetCurrent(new MockLoggerFactory());
            ValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
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
            ILogger logger = await LoggerFactory.Create();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public async Task LoggingCanCallDebugMethodth()
        {
            ILogger logger = await LoggerFactory.Create();

            await logger.Debug(null);
            await logger.Debug(string.Empty);
            await logger.Debug(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallFatalMethodth()
        {
            ILogger logger = await LoggerFactory.Create();

            await logger.Fatal(string.Empty);
            await logger.Fatal(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogErrorMethodth()
        {
            ILogger logger = await LoggerFactory.Create();

            await logger.LogError(string.Empty);
            await logger.LogError(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogInfoMethodth()
        {
            ILogger logger = await LoggerFactory.Create();

            await logger.LogInfo(string.Empty);
            await logger.LogInfo(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task ValidatorCanCallIsValidMethodth()
        {
            IValidator validator = await ValidatorFactory.Create();

            Assert.IsTrue(validator.IsValid(string.Empty));
        }

        [TestMethod]
        public async Task ValidatorCanCallGetErrorMessagesMethodth()
        {
            IValidator validator = await ValidatorFactory.Create();

            Assert.AreEqual(Enumerable.Empty<string>().Count(), validator.GetErrorMessages(string.Empty).Count());
        }
    }
}
