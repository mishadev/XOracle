﻿using System;
using XOracle.Data.Core;

namespace XOracle.Application.Core
{
    public class GetAccountRequest
    {
        public Guid? AccountId { get; set; }

        public string Name { get; set; }

        public DetalizationLevel DetalizationLevel { get; set; }
    }
}
