using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class AccountSet : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = { 
                this.IsDefault(this.AccountId, "AccountId"),
            };

            return results;
        }
    }

    [Serializable]
    public partial class AccountSetAccounts : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = { 
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.AccountSetId, "AccountSetId")
            };

            return results;
        }
    }
}
