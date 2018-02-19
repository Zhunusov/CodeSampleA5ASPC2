using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Utils
{
    public static class AttributeValidator
    {
        public static List<string> Validation(object entity)
        {
            var errors = new List<string>();
            var results = new List<ValidationResult>();
            var context = new ValidationContext(entity);

            if (Validator.TryValidateObject(entity, context, results, true)) return null;

            errors.AddRange(results.Select(error => error.ErrorMessage));
            return errors;
        }
    }
}
