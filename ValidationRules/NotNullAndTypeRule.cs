using System;
using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules
{
	public class NotNullAndTypeRule : ValidationRule
	{
		public Type ExpectedType { get; set; }

		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value == null)
				return new ValidationResult(false, "Value cannot be empty.");

			if (!ExpectedType.IsInstanceOfType(value))
				return new ValidationResult(false, $"Value must be of type {ExpectedType.Name}.");

			return ValidationResult.ValidResult;
		}
	}
}
