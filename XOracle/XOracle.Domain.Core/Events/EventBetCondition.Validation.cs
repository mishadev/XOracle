using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class EventBetCondition : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.IsDefault(this.CloseDate, "CloseDate"),
                this.IsDefault(this.CurrencyTypeId, "CurrencyTypeId"),
                this.IsDefault(this.EventBetRateAlgorithmId, "EventBetRateAlgorithmId"),
            };

            return results;
        }
    }
}
