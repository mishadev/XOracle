using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class BetRateAlgorithm : IValidatableObject
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
