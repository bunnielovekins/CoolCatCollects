using System;
using System.Web;

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

		public static bool IsEmpty(this string str)
		{
			return string.IsNullOrEmpty(str);
		}

		public static IHtmlString FormatNewLines(string str)
		{
			return new HtmlString(str.Replace("\n", "<br/>"));
		}
	}
}
