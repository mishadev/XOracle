using XOracle.Application;
using XOracle.Data;
using XOracle.Data.Core;
using XOracle.Domain.Services;

namespace XOracle.Console
{
    class Program
    {
        static void Main(string[] args)
        {

        }

        static void CreateAccout()
        {
            var service = new AccountsService(new AccountsDomainService(new Repository<Account>(new InmemoryUnitOfWork())));

            service.CreateAccount(null);
        }
    }
}
