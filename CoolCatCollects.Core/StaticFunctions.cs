using System;
using System.Web;

namespace CoolCatCollects.Core
{
	public static class StaticFunctions
	{
		/// <summary>
		/// Formats a string into a decimal with 2 dp
		/// </summary>
		/// <param name="currency">String to format, e.g. "3.5964"</param>
		/// <returns>3.14</returns>
		public static decimal FormatCurrency(string currency)
		{
			var d = decimal.Parse(currency);
			return Math.Round(d, 2);
		}

		/// <summary>
		/// Takes a decimal, formats it to a currency
		/// </summary>
		/// <param name="currency"></param>
		/// <returns>"£3.14"</returns>
		public static string FormatCurrencyStr(decimal currency)
		{
			return Math.Abs(currency).ToString("C");
		}

		/// <summary>
		/// Takes a string, formats it to a currency
		/// </summary>
		/// <param name="currency"></param>
		/// <returns>£3.14</returns>
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

		/// <summary>
		/// Formats new lines into <br/>
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		public static IHtmlString FormatNewLines(string str)
		{
			return new HtmlString(str.Replace("\n", "<br/>"));
		}
	}
}
