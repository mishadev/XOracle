﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class AccountLogin : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.ProviderKey, "ProviderKey"),
            }; 

            return results;
        }
    }
}
