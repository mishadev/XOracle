using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Data
{
    public partial class Event : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.StartDate == default(DateTime))
                results.Add(new ValidationResult("StartDate", new string[] { "StartDate" }));

            if (this.EndDate == default(DateTime))
                results.Add(new ValidationResult("EndDate", new string[] { "EndDate" }));

            if (this.LockDate == default(DateTime))
                results.Add(new ValidationResult("LockDate", new string[] { "LockDate" }));

            if (this.StartDate <= this.EndDate)
                results.Add(new ValidationResult("StartDate and EndDate", new string[] { "StartDate", "EndDate" }));

            if (this.LockDate <= this.EndDate && this.LockDate >= this.StartDate)
                results.Add(new ValidationResult("LockDate", new string[] { "LockDate" }));

            if (this.CreatorAccountId == Guid.Empty)
                results.Add(new ValidationResult("CreatorAccountId", new string[] { "CreatorAccountId" }));

            if (this.AccessibilityConditionId == Guid.Empty)
                results.Add(new ValidationResult("AccessibilityConditionId", new string[] { "AccessibilityConditionId" }));

            if (this.JudgingConditionId == Guid.Empty)
                results.Add(new ValidationResult("JudgingConditionId", new string[] { "JudgingConditionId" }));

            if (this.VictoryConditionId == Guid.Empty)
                results.Add(new ValidationResult("VictoryConditionId", new string[] { "VictoryConditionId" }));

            if (this.BetConditionId == Guid.Empty)
                results.Add(new ValidationResult("BetConditionId", new string[] { "BetConditionId" }));

            return results;
        }
    }
}
