using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XOracle.Data.Core;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using System.Threading.Tasks;

namespace XOracle.Data.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            ValidatorFactory.SetCurrent(new DataAnnotationsEntityValidatorFactory());
        }

        [TestMethod]
        public void IdentityGeneratorCanGenerat()
        {
            var guid = IdentityGenerator.NewSequentialGuid();

            Assert.AreNotEqual(Guid.Empty, guid);
        }

        [TestMethod]
        public void NewEventEnsureIdentiry()
        {
            Event @event = new Event();
            @event.EnsureIdentity();

            Assert.IsFalse(@event.IsTransient());
        }

        [TestMethod]
        public void NewEventIsTransient()
        {
            Event @event = new Event();

            Assert.IsTrue(@event.IsTransient());
        }

        [TestMethod]
        public async Task NewEventIsNotValid()
        {
            Event @event = new Event();
            IValidator validator = await ValidatorFactory.Create();

            Assert.IsFalse(validator.IsValid(@event));
        }
    }
}
