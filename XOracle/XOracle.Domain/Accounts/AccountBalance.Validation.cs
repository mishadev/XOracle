using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain
{
    [Serializable]
    public partial class AccountBalance : IValidatableObject
    {
        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.AccountId == Guid.Empty)
                results.Add(new ValidationResult("AccountId", new[] { "AccountId" }));

            if (this.ValueTypeId == Guid.Empty)
                results.Add(new ValidationResult("ValueTypeId", new[] { "ValueTypeId" }));

            if (this.Value < 0)
                results.Add(new ValidationResult("Value", new[] { "Value" }));

            return results;
        }
    }
}
