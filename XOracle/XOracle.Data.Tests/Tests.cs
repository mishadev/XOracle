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

        [TestMethod]
        public async Task CreateUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            Assert.IsNotNull(set);
        }

        [TestMethod]
        public async Task FillByOneTypeUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            Assert.AreEqual(1, set.Count);
        }

        [TestMethod]
        public async Task TestByRelationsUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();
            var set2 = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            Assert.AreEqual(set2.Count, set.Count);
        }

        [TestMethod]
        public async Task TestByRelationsUnitOfWork2()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();
            var count = set.Count;

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.CreateSet<Account>();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task CommitByRelationsUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            var count = set.Count;

            await unitOfWork.Commit();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task RollbackUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();
            var count = set.Count;

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.Rollback();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task CommitRollbackUnitOfWork()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.Commit();

            var count = set.Count;

            await unitOfWork.Rollback();

            set = await unitOfWork.CreateSet<Account>();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task CommitRollbackUnitOfWork1()
        {
            InmemoryUnitOfWork unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.Commit();

            var set1 = await unitOfWork.CreateSet<Account>();
            var count = set1.Count;

            var account1 = new Account();
            account1.EnsureIdentity();

            set1.Add(account1.Id, account1);

            await unitOfWork.Rollback();

            var set3 = await unitOfWork.CreateSet<Account>();

            Assert.AreEqual(count, set3.Count);
        }
    }
}
