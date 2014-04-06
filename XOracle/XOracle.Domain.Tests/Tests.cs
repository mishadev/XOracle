using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using XOracle.Azure.Core.Helpers;
using XOracle.Data.Azure;
using XOracle.Data.Azure.Entities;

namespace XOracle.Domain.Tests
{
    [TestClass]
    public class Tests
    {
        [TestInitialize]
        public void Initialize()
        { }

        [TestMethod]
        public async Task Transfer()
        {
            var acc = new Account { Email = "smahs", Name = "misha" };
            acc.EnsureIdentity();

            var accb = new AccountBalance { AccountId = acc.Id };
            accb.EnsureIdentity();

            var accountId = accb.AccountId;

            var f = AzureTransferExpression<AzureAccount, Account>.Transfer(a => a.Name == "misha" && a.Id == accountId);
            var objs = new[] { new AzureAccount { Name = "misha", Email = "smahs", Id = acc.Id } };

            var answer = objs.Where(f.Compile());

            Assert.IsTrue(answer.Any());
        }


    }
}
