using XOracle.Domain.Core;

namespace XOracle.Domain.Core
{
    public partial class Account : Entity
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
