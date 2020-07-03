using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Models
{
	public class NewPurchaseModel
	{
		public int Id { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
		public DateTime Date { get; set; }
		[DisplayName("Set Number")]
		public string SetNumber { get; set; }
		[DisplayName("Set Name")]
		public string SetName { get; set; }
		public string Theme { get; set; }
		public string Promotions { get; set; }
		[DisplayName("Total Paid")]
		[DataType(DataType.Currency)]
		public decimal Price { get; set; }
		[DisplayName("Unit Price")]
		[DataType(DataType.Currency)]
		public decimal UnitPrice { get; set; }
		[DisplayName("Number Bought")]
		public int Quantity { get; set; }
		[DisplayName("Parts Per Set")]
		public int Parts { get; set; }
		[DisplayName("Total Parts")]
		public int TotalParts { get; set; }
		[DisplayName("Minifigure Value")]
		public decimal MinifigureValue { get; set; }
		[DisplayName("Price to Part Out Ratio")]
		public decimal PriceToPartOutRatio { get; set; }
		public string Source { get; set; }
		[DisplayName("Payment Method")]
		public string PaymentMethod { get; set; }
		[DisplayName("Average Part Out Value")]
		[DataType(DataType.Currency)]
		public decimal AveragePartOutValue { get; set; }
		[DisplayName("My Part Out Value (Bricks + Minifigures)")]
		[DataType(DataType.Currency)]
		public decimal MyPartOutValue { get; set; }
		[DisplayName("Expected Gross Profit")]
		[DataType(DataType.Currency)]
		public decimal ExpectedGrossProfit { get; set; }
		[DisplayName("Expected Net Profit")]
		[DataType(DataType.Currency)]
		public decimal ExpectedNetProfit { get; set; }
		public string Status { get; set; }
		[DisplayName("Selling Notes")]
		[DataType(DataType.MultilineText)]
		public string SellingNotes { get; set; }
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		public static class Statuses
		{
			public const string InTransit = "In Transit";
			public const string StockRoom = "Stockroom";
			public const string Listed = "Listed";
			public const string Complete = "Complete";

			public static IEnumerable<string> All = new List<string>
			{
				InTransit,
				StockRoom,
				Listed,
				Complete
			};
		}
	}
}
