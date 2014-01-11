using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace XOracle.Azure.Web.Front
{
    public class IdentityAccount : IUser
    {
        public IdentityAccount()
        {
            Logins = new List<IdentityAccountLogin>();
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public IList<IdentityAccountLogin> Logins { get; set; }
    }
}