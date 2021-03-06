﻿using System;

namespace XOracle.Application.Core
{
    public class CalculateBetRateResponse
    {
        public DateTime CreationDate { get; set; }

        public decimal Rate { get; set; }

        public decimal WinRate { get; set; }

        public decimal WinValue { get; set; }
    }
}
