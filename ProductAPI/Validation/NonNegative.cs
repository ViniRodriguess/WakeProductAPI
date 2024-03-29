using System;
using System.ComponentModel.DataAnnotations;

namespace ProductAPI.Validation
{
    public class NonNegativeAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                decimal price = (decimal)value;
                if (price < 0)
                {
                    return new ValidationResult("O valor do produto não pode ser negativo");
                }
            }

            return ValidationResult.Success;
        }
    }
}
