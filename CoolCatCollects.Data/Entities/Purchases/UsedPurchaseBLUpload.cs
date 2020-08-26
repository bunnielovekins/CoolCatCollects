using System;

namespace CoolCatCollects.Data.Entities.Purchases
{
	public class UsedPurchaseBLUpload : BaseEntity
	{
		public virtual UsedPurchase UsedPurchase { get; set; }
		public DateTime Date { get; set; }
		public int Parts { get; set; }
		public int Lots { get; set; }
		public decimal Value { get; set; }
		public string Notes { get; set; }
	}
}
