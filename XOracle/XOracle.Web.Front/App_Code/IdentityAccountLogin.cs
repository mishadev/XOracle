namespace XOracle.Web.Front
{
    public class IdentityAccountLogin
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public IdentityAccount User { get; set; }
        public string UserId { get; set; }
    }
}