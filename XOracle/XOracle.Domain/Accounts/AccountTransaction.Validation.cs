using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain
{
    [Serializable]
    public partial class AccountTransaction : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.AccountId == Guid.Empty)
                results.Add(new ValidationResult("AccountId", new[] { "AccountId" }));

            if (this.ValueTypeId == Guid.Empty)
                results.Add(new ValidationResult("ValueTypeId", new[] { "ValueTypeId" }));

            if (this.Date != default(DateTime))
                results.Add(new ValidationResult("Date", new[] { "Date" }));

            return results;
        }
    }
}
