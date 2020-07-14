namespace CoolCatCollects.Data.Entities.Purchases
{
	public class UsedPurchaseWeight : BaseEntity
	{
		public virtual UsedPurchase UsedPurchase { get; set; }
		public string Colour { get; set; }
		public decimal Weight { get; set; }
	}
}
