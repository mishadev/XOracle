using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace XOracle.Domain.Core
{
    public static class ValidationExtention
    {
        public static IEnumerable<ValidationResult> ValidateEnum(this IEnum @enum, ValidationContext validationContext)
        {
            ValidationResult[] results = { 
                ((IValidatableObject)@enum).Is(v => string.IsNullOrWhiteSpace(v), @enum.Name, "Name")
            };

            return results;
        }

        public static ValidationResult Is<TValue>(this IValidatableObject validatable, Func<TValue, bool> predicate, TValue value, string name)
        {
            return predicate != null && predicate(value) ? new ValidationResult(validatable.GetType().Name + "." + name, new[] { name }) : null;
        }

        public static ValidationResult IsDefault<TValue>(this IValidatableObject validatable, TValue value, string name)
        {
            return validatable.Is(v => v != null && v.Equals(default(TValue)), value, name);
        }
    }
}
