using Infrastructure.Validation;
using System.ComponentModel.DataAnnotations;

namespace ManagmentSystem.Infrastructure.Validation.Attribute;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class StringValidateAttribute : ValidationAttribute
{
    readonly int min, max; readonly bool isnull;

    public StringValidateAttribute(int min = -1, int max = -1, bool isnull = false)
    {
        this.min = min; this.max = max; this.isnull = isnull;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (isnull && (value == null || string.IsNullOrWhiteSpace((string)value)))
            return ValidationResult.Success;

        if (value == null || string.IsNullOrWhiteSpace((string)value))
            return new ValidationResult(ErrorCode.Required);

        if (min != -1 && ((string)value).Length < min)
            return new ValidationResult(ErrorCode.MinimumLengthAllowedIs(min));

        if (max != -1 && ((string)value).Length > max)
            return new ValidationResult(ErrorCode.MaximumLengthAllowedIs(max));

        return ValidationResult.Success;
    }
}