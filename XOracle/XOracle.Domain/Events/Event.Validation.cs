using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Domain.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class Event : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.StartDate > this.EndDate ? new ValidationResult("StartDate and EndDate", new[] { "StartDate", "EndDate" }) : null,

                this.Is(title => string.IsNullOrWhiteSpace(title) || title.Length >= 1 << 6, this.Title, "Title"),

                //this.IsDefault(this.ImageId, "ImageId"),
                this.IsDefault(this.StartDate, "StartDate"),
                this.IsDefault(this.EndDate, "EndDate"),
                this.IsDefault(this.AccountId, "CreatorAccountId"),
                this.IsDefault(this.EventRelationTypeId, "EventRelationTypeId"),
                this.IsDefault(this.ArbiterAccountSetId, "ArbiterAccountSetId"),
                this.IsDefault(this.ExpectedEventConditionId, "ExpectedEventConditionId"),
                this.IsDefault(this.EventBetConditionId, "EventBetConditionId"),
            };

            return results;
        }
    }
}
