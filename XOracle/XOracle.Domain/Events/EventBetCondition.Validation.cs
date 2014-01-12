using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class EventBetCondition : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.CloseDate, "LockDate"),
                this.IsDefault(this.CurrencyTypeId, "CurrencyTypeId"),
                this.IsDefault(this.EventBetRateAlgorithmId, "EventBetRateAlgorithmId"),
            };

            return results;
        }
    }
}
