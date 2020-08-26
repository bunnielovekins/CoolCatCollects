using System;
using System.Collections.Generic;

namespace CoolCatCollects.Data.Entities.Purchases
{
	public class UsedPurchase : BaseEntity
	{
		public DateTime Date { get; set; }
		public string Source { get; set; }
		public string SourceUsername { get; set; }
		public string OrderNumber { get; set; }
		public string Price { get; set; }
		public string PaymentMethod { get; set; }
		public bool Receipt { get; set; }
		public string DistanceTravelled { get; set; }
		public string Location { get; set; }
		public decimal Postage { get; set; }
		public decimal Weight { get; set; }
		public decimal PricePerKilo { get; set; }
		public bool CompleteSets { get; set; }
		public string Notes { get; set; }
		/// <summary>
		/// Contents of the Used Purchase
		/// </summary>
		public virtual ICollection<UsedPurchaseWeight> Weights { get; set; }
		public virtual ICollection<UsedPurchaseBLUpload> BLUploads { get; set; }
	}
}
