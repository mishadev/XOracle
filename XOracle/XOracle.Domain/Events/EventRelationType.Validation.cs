using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class EventRelationType : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            return this.ValidateEnum(validationContext);
        }
    }
}
