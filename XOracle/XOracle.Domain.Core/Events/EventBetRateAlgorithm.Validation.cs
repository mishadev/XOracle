using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class EventBetRateAlgorithm : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = { 
                this.IsDefault(this.AlgorithmTypeId, "AlgorithmTypeId"),
                this.Is(v => v < 0, this.StartRate, "StartRate"),
                this.Is(v => v < 0, this.EndRate, "EndRate"),
            };

            return results;
        }
    }
}
