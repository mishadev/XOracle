using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Data.Core;
using XOracle.Domain;
using XOracle.Infrastructure;
using XOracle.Infrastructure.Core;
using XOracle.Infrastructure.Utils;

namespace XOracle.Data.Tests
{
    [TestClass]
    public class Tests
    {
        private string _emailFormat = "misha_dev{0}@live.com";

        private string UniqueEmail
        {
            get
            {
                return string.Format(this._emailFormat, Guid.NewGuid().GetHashCode());
            }
        }

        [TestInitialize]
        public void Initialize()
        {
            Factory<IValidator>.SetCurrent(new DataAnnotationsEntityValidatorFactory());
            Factory<ILogger>.SetCurrent(new MockLoggerFactory());
            Factory<IBinarySerializer>.SetCurrent(new NetBinarySerializerFactory());
            Factory<IEmailAddressValidator>.SetCurrent(new RegexEmailAddressValidatorFactory());
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
        public void NewEventIsNotValid()
        {
            Event @event = new Event();
            IValidator validator = Factory<IValidator>.GetInstance();

            Assert.IsFalse(validator.IsValid(@event));
        }

        [TestMethod]
        public async Task CreateUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            Assert.IsNotNull(set);
        }

        [TestMethod]
        public async Task FillByOneTypeUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            Assert.AreEqual(1, set.Count);
        }

        [TestMethod]
        public async Task TestByRelationsUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
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
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();
            var count = set.Count;

            var account = new Account();
            account.EnsureIdentity();

            set.Add(account.Id, account);

            Assert.AreEqual(count + 1, set.Count);
        }

        [TestMethod]
        public async Task CommitByRelationsUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account { Email = this.UniqueEmail, Name = "misha" };
            account.EnsureIdentity();

            set.Add(account.Id, account);

            var count = set.Count;

            await unitOfWork.Commit();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task RollbackUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var count = set.Count;
            var account = new Account { Email = this.UniqueEmail, Name = "misha" };
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.Rollback();

            Assert.AreEqual(count, set.Count);
        }

        [TestMethod]
        public async Task CommitRollbackUnitOfWork()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account { Email = this.UniqueEmail, Name = "misha" };
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
            var unitOfWork = new InmemoryUnitOfWork();
            var set = await unitOfWork.CreateSet<Account>();

            var account = new Account { Email = this.UniqueEmail, Name = "misha" };
            account.EnsureIdentity();

            set.Add(account.Id, account);

            await unitOfWork.Commit();

            var set1 = await unitOfWork.CreateSet<Account>();
            var count = set1.Count;

            var account1 = new Account { Email = this.UniqueEmail, Name = "misha" };
            account1.EnsureIdentity();

            set1.Add(account1.Id, account1);

            await unitOfWork.Rollback();

            var set3 = await unitOfWork.CreateSet<Account>();

            Assert.AreEqual(count, set3.Count);
        }

        [TestMethod]
        public async Task CurrencyTypeRepository()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var repo = new Repository<CurrencyType>(unitOfWork);

            var currencyType = await repo.GetBy(n => n.Name == CurrencyType.Reputation);

            Assert.AreEqual(CurrencyType.Reputation, currencyType.Name);
        }

        [TestMethod]
        public async Task AlgorithmTypeAllRepository()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var repo = new Repository<AlgorithmType>(unitOfWork);

            var algorithmTypes = await repo.GetFiltered(n => true);

            Assert.AreEqual(2, algorithmTypes.Count());
        }

        [TestMethod]
        public async Task AlgorithmTypeRepository()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var repo = new Repository<AlgorithmType>(unitOfWork);

            var algorithmType = await repo.GetBy(n => n.Name == AlgorithmType.Linear);

            Assert.AreEqual(AlgorithmType.Linear, algorithmType.Name);

            algorithmType = await repo.GetBy(n => n.Name == AlgorithmType.Exponential);

            Assert.AreEqual(AlgorithmType.Exponential, algorithmType.Name);
        }

        [TestMethod]
        public async Task EventRelationTypeAllRepository()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var repo = new Repository<EventRelationType>(unitOfWork);

            var eventRelationTypes = await repo.GetFiltered(n => true);

            Assert.AreEqual(3, eventRelationTypes.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ScopeableUnitOfWorkFactoryEx()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (new InmemoryUnitOfWork())
            {
                using (f.Create())
                {
                    await repo.Add(new EventRelationType { Name = string.Empty });
                }
            }
        }

        [TestMethod]
        public async Task ScopeableUnitOfWorkFactory()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (f.Create())
            {
                await repo.Add(new EventRelationType { Name = "x1" });
            }

            Assert.AreEqual(count + 1, (await repo.GetFiltered(x => true)).Count());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task ScopeableUnitOfWorkFactorySubEx()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (f.Create())
            {
                await repo.Add(new EventRelationType { Name = "x3" });
                using (f.Create())
                {
                    await repo.Add(new EventRelationType { Name = string.Empty });
                }
            }
        }

        [TestMethod]
        public async Task ScopeableUnitOfWorkFactorySubRollback()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (f.Create())
            {
                await repo.Add(new EventRelationType { Name = "x4" });
                using (f.Create())
                {
                    await repo.Add(new EventRelationType { Name = "x5" });
                }
                await unitOfWork.Rollback();
            }

            Assert.AreEqual(count, (await repo.GetFiltered(x => true)).Count());
        }

        [TestMethod]
        public async Task ScopeableUnitOfWorkFactorySub()
        {
            var unitOfWork = new InmemoryUnitOfWork();
            var f = new ScopeableUnitOfWorkFactory(unitOfWork);
            var repo = new Repository<EventRelationType>(unitOfWork);

            var count = (await repo.GetFiltered(x => true)).Count();

            using (f.Create())
            {
                await repo.Add(new EventRelationType { Name = "x6" });
                using (f.Create())
                {
                    await repo.Add(new EventRelationType { Name = "x7" });
                }
            }

            Assert.AreEqual(count + 2, (await repo.GetFiltered(x => true)).Count());
        }
    }
}
