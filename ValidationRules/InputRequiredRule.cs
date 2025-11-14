using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules
{
	public class InputRequiredRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return !string.IsNullOrWhiteSpace(value as string)
				? ValidationResult.ValidResult
				: new ValidationResult(false, "This field is required.");
		}
	}
}
