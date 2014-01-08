using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class Bet : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.IsDefault(this.EventId, "EventId"),
                this.IsDefault(this.CurrencyTypeId, "CurrencyTypeId"),
                this.Is(v => v <= 0, this.Value, "Value"),
                this.Is(d => d < DateTime.Now && d == default(DateTime), this.CreationDate, "CreationDate"),
                this.IsDefault(this.AccountId, "AccountId"),
                this.IsDefault(this.OutcomesTypeId, "OutcomesTypeId")
            };

            return results;
        }
    }
}
