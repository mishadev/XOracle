using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class AccountSet
    { }

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
