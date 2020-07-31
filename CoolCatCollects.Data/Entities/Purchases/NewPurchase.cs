using System;
using System.ComponentModel;

namespace CoolCatCollects.Data.Entities.Purchases
{
	public class NewPurchase : BaseEntity
	{
		public DateTime Date { get; set; }
		[DisplayName("Set Number")]
		public string SetNumber { get; set; }
		[DisplayName("Set Name")]
		public string SetName { get; set; }
		public string Theme { get; set; }
		public string Promotions { get; set; }
		[DisplayName("Total Paid")]
		public decimal Price { get; set; }
		[DisplayName("Unit Price")]
		public decimal UnitPrice { get; set; }
		[DisplayName("Number Bought")]
		public int Quantity { get; set; }
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
		[DisplayName("Average Part Out Value (no minifigs)")]
		public decimal AveragePartOutValue { get; set; }
		[DisplayName("My Part Out Value (Bricks + Minifigures)")]
		public decimal MyPartOutValue { get; set; }
		[DisplayName("Expected Gross Profit")]
		public decimal ExpectedGrossProfit { get; set; }
		[DisplayName("Expected Net Profit")]
		public decimal ExpectedNetProfit { get; set; }
		public string Status { get; set; }
		[DisplayName("Selling Notes")]
		public string SellingNotes { get; set; }
		public string Notes { get; set; }
		public string Receipt { get; set; }
	}
}
