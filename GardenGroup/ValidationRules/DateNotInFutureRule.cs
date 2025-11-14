using System;
using System.Globalization;
using System.Windows.Controls;

namespace UI.ValidationRules
{
	public class DateNotInFutureRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			return value is DateTime date && date <= DateTime.Today
				? ValidationResult.ValidResult
				: new ValidationResult(false, "Please select a valid date.");
		}
	}
}
