using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class ValueType : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (string.IsNullOrWhiteSpace(this.Name))
                results.Add(new ValidationResult("Name", new[] { "Name" }));

            return results;
        }
    }
}
