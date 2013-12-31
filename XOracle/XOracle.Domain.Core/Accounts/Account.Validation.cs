using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using XOracle.Infrastructure.Core;

namespace XOracle.Domain.Core
{
    [Serializable]
    public partial class Account : IValidatableObject
    {
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            ValidationResult[] results = {
                this.Is(email => !this.IsEmailAddressValid(email).GetAwaiter().GetResult(), this.Email, "Email"),
                this.Is(name => string.IsNullOrWhiteSpace(name), this.Name, "Name"),
            };

            return results;
        }

        private async Task<bool> IsEmailAddressValid(string email)
        {
            var validator = await Factory<IEmailAddressValidator>.GetInstance();

            return await validator.IsValid(email);
        }
    }
}
