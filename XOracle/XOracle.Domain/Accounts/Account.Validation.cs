using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using XOracle.Domain.Core;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain
{
    [Serializable]
    public partial class Account : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.Is(email => email != null && !this.IsEmailAddressValid(email).GetAwaiter().GetResult(), this.Email, "Email"),
                this.Is(name => string.IsNullOrWhiteSpace(name), this.Name, "Name"),
            };

            return results;
        }

        private async Task<bool> IsEmailAddressValid(string email)
        {
            var validator = Factory<IEmailAddressValidator>.GetInstance();

            return await validator.IsValid(email);
        }
    }
}
