using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class Event : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.StartDate <= this.EndDate ? new ValidationResult("StartDate and EndDate", new[] { "StartDate", "EndDate" }) : null,
                this.LockDate <= this.EndDate && this.LockDate >= this.StartDate ? new ValidationResult("LockDate", new[] { "LockDate" }) : null,

                this.Is(title => string.IsNullOrWhiteSpace(title) || title.Length >= 1 << 6, this.Title, "Title"),
                this.Is(desc => string.IsNullOrWhiteSpace(desc) || desc.Length >= 1 << 8, this.Description, "Description"),

                this.IsDefault(this.ImageId, "ImageId"),
                this.IsDefault(this.StartDate, "StartDate"),
                this.IsDefault(this.EndDate, "EndDate"),
                this.IsDefault(this.LockDate, "LockDate"),
                this.IsDefault(this.CreatorAccountId, "CreatorAccountId"),
                this.IsDefault(this.EventRelationTypeId, "EventRelationTypeId"),
                this.IsDefault(this.JudgingAccountSetId, "JudgingAccountSetId"),
                this.IsDefault(this.ExpectedEventConditionId, "ExpectedEventConditionId"),
                this.IsDefault(this.EventBetConditionId, "EventBetConditionId"),
            };

            return results;
        }
    }
}
