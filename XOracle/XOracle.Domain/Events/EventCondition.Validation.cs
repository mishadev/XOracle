using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class EventCondition : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.Is(desc => string.IsNullOrWhiteSpace(desc) || desc.Length >= 1 << 10, this.Description, "Description"),
            };

            return results;
        }
    }
}
