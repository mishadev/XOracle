﻿using System;

namespace XOracle.Application.Core
{
    public class GetDetailsAccountResponse
    {
        public Guid AccountId { get; set; }

        public double Reputation { get; set; }

        public string Name { get; set; }

        public string EMail { get; set; }
    }
}
