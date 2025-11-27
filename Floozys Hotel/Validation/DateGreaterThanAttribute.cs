using System;
using System.ComponentModel.DataAnnotations;

namespace Floozys_Hotel.Validation
{
    /// <summary>
    /// Custom validation attribute that checks if a DateTime property is greater than another DateTime property
    /// </summary>
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        // Constructor - får navnet på property vi skal sammenligne med (fx "StartDate")
        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        // IsValid kaldes automatisk når validation køres
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Hent property info for comparison property (fx StartDate)
            var propertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (propertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            // Hent værdien af comparison property (StartDate's værdi)
            var comparisonValue = propertyInfo.GetValue(validationContext.ObjectInstance);

            // Sammenlign datoerne
            if (value is DateTime currentValue && comparisonValue is DateTime comparisonDate)
            {
                if (currentValue <= comparisonDate)
                {
                    // Validation FAILED - dato er IKKE større
                    return new ValidationResult(
                        ErrorMessage ?? $"{validationContext.DisplayName} must be after {_comparisonProperty}"
                    );
                }
            }

            // Validation SUCCESS
            return ValidationResult.Success;
        }
    }
}