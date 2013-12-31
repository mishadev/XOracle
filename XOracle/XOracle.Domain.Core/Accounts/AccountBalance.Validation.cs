﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class AccountBalance : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.Is(value => value < 0, this.Value, "Value"),
                     
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.CurrencyTypeId, "CurrencyTypeId"),                
            };

            return results;
        }
    }
}
