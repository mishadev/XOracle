namespace XOracle.Web.Front
{
    public class IdentityAccountLogin
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual IdentityAccount User { get; set; }
        public virtual string UserId { get; set; }
    }
}