﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class AccountTransaction : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.ValueTypeId, "ValueTypeId"),
                this.IsDefault(this.Date, "Date"),
            };

            return results;
        }
    }
}
