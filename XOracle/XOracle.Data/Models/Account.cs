using System;
using XOracle.Data.Core;

namespace XOracle.Data
{
    [Serializable]
    public class Account : Entity
    {
        public string EMail { get; set; }
    }
}
