using System;
using System.ComponentModel;

namespace CoolCatCollects.Data.Entities.Purchases
{
	public class UsedPurchase : BaseEntity
	{
		public DateTime Date { get; set; }
		public string Source { get; set; }
		[DisplayName("Source Username")]
		public string SourceUsername { get; set; }
		[DisplayName("Order Number")]
		public string OrderNumber { get; set; }
		[DisplayName("Price Paid")]
		public string Price { get; set; }
		[DisplayName("Payment Method")]
		public string PaymentMethod { get; set; }
		public bool Receipt { get; set; }
		[DisplayName("Distance Travelled")]
		public string DistanceTravelled { get; set; }
		public string Location { get; set; }
		public decimal Postage { get; set; }
		[DisplayName("Weight (KG)")]
		public decimal Weight { get; set; }
		[DisplayName("Price per Kilo (£)")]
		public decimal PricePerKilo { get; set; }
		[DisplayName("Complete Sets?")]
		public bool CompleteSets { get; set; }
		public string Notes { get; set; }
	}
}
