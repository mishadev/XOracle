using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class Account : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            if (this.IsEmailAddressValid(this.Email))
                results.Add(new ValidationResult("Email", new[] { "Email" }));

            if (string.IsNullOrWhiteSpace(this.Name))
                results.Add(new ValidationResult("Name", new[] { "Name" }));

            return results;
        }

        private bool IsEmailAddressValid(string email)
        {
            var validator = Factory<IEmailAddressValidator>.GetInstance().GetAwaiter().GetResult();

            return validator.IsValid(email).GetAwaiter().GetResult();
        }
    }
}
