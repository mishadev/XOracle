using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using XOracle.Infrastructure.Core;

namespace XOracle.Infrastructure
{
    public class DataAnnotationsEntityValidator : IValidator
    {
        public bool IsValid<TEntity>(TEntity item)
            where TEntity : class
        {
            IEnumerable<string> errors = GetErrorMessages(item);

            return !errors.Any();
        }

        public IEnumerable<string> GetErrorMessages<TEntity>(TEntity item)
            where TEntity : class
        {
            if (item == null)
                return null;

            IEnumerable<string> objectErrors = GetObjectErrors(item);
            IEnumerable<string> attributeErrors = GetAttributeErrors(item);

            return objectErrors.Union(attributeErrors).ToArray();
        }

        private IEnumerable<string> GetObjectErrors<TEntity>(TEntity item)
            where TEntity : class
        {
            var results = Enumerable.Empty<string>();

            if (item is IValidatableObject)
            {
                var context = new ValidationContext(item, null, null);
                var validationResults = ((IValidatableObject)item).Validate(context);

                results = validationResults
                    .Where(vr => vr != null)
                    .Select(vr => vr.ErrorMessage);
            }

            return results;
        }

        private IEnumerable<string> GetAttributeErrors<TEntity>(TEntity item)
            where TEntity : class
        {
            var results = from property in TypeDescriptor.GetProperties(item).Cast<PropertyDescriptor>()
                          from attribute in property.Attributes.OfType<ValidationAttribute>()
                          where !attribute.IsValid(property.GetValue(item))
                          select attribute.FormatErrorMessage(string.Empty);

            return results;
        }
    }
}
