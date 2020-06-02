using System;

namespace CoolCatCollects.Core
{
	public static class StaticFunctions
	{
		public static decimal FormatCurrency(string currency)
		{
			var d = decimal.Parse(currency);
			return Math.Round(d, 2);
		}

		public static string FormatCurrencyStr(decimal currency)
		{
			return Math.Abs(currency).ToString("C");
		}

		public static string FormatCurrencyStr(string currency)
		{
			var d = decimal.Parse(currency);
			if (d == 0)
			{
				return "£0.00";
			}
			return FormatCurrencyStr(d);
		}
	}
}
