using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain
{
    public partial class Event : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.StartDate == default(DateTime))
                results.Add(new ValidationResult("StartDate", new[] { "StartDate" }));

            if (this.EndDate == default(DateTime))
                results.Add(new ValidationResult("EndDate", new[] { "EndDate" }));

            if (this.LockDate == default(DateTime))
                results.Add(new ValidationResult("LockDate", new[] { "LockDate" }));

            if (this.StartDate <= this.EndDate)
                results.Add(new ValidationResult("StartDate and EndDate", new[] { "StartDate", "EndDate" }));

            if (this.LockDate <= this.EndDate && this.LockDate >= this.StartDate)
                results.Add(new ValidationResult("LockDate", new[] { "LockDate" }));

            if (this.CreatorAccountId == Guid.Empty)
                results.Add(new ValidationResult("CreatorAccountId", new[] { "CreatorAccountId" }));

            if (this.AccessibilityConditionId == Guid.Empty)
                results.Add(new ValidationResult("AccessibilityConditionId", new[] { "AccessibilityConditionId" }));

            if (this.JudgingConditionId == Guid.Empty)
                results.Add(new ValidationResult("JudgingConditionId", new[] { "JudgingConditionId" }));

            if (this.VictoryConditionId == Guid.Empty)
                results.Add(new ValidationResult("VictoryConditionId", new[] { "VictoryConditionId" }));

            if (this.BetConditionId == Guid.Empty)
                results.Add(new ValidationResult("BetConditionId", new[] { "BetConditionId" }));

            return results;
        }
    }
}
