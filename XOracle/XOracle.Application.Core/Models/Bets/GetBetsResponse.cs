﻿using System;
using System.Collections.Generic;

namespace XOracle.Application.Core
{
    public class GetBetsResponse
    {
        public IEnumerable<GetBetResponse> Bets { get; set; }
    }
}
