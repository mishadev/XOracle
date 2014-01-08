using XOracle.Domain.Core;

namespace XOracle.Domain
{
    public partial class Account : Entity
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
