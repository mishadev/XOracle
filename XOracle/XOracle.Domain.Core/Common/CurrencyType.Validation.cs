using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class CurrencyType : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            return this.ValidateEnum(validationContext);
        }
    }
}
