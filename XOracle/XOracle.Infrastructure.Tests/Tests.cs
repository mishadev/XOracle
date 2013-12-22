using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Threading.Tasks;

using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Core.Logging;
using XOracle.Infrastructure.Logging;

namespace XOracle.Infrastructure.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            LoggerFactory.SetCurrent(new MockLoggerFactory());
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
            ILogger logger = await LoggerFactory.CreateLog();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public async Task LoggingCanCallDebugMethodth()
        {
            ILogger logger = await LoggerFactory.CreateLog();

            await logger.Debug(null);
            await logger.Debug(string.Empty);
            await logger.Debug(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallFatalMethodth()
        {
            ILogger logger = await LoggerFactory.CreateLog();

            await logger.Fatal(string.Empty);
            await logger.Fatal(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogErrorMethodth()
        {
            ILogger logger = await LoggerFactory.CreateLog();

            await logger.LogError(string.Empty);
            await logger.LogError(string.Empty, (Exception)null);
        }

        [TestMethod]
        public async Task LoggingCanCallLogInfoMethodth()
        {
            ILogger logger = await LoggerFactory.CreateLog();

            await logger.LogInfo(string.Empty);
            await logger.LogInfo(string.Empty, (Exception)null);
        }
    }
}
