namespace CoolCatCollects.Data.Entities
{
	public class BricklinkOrder : Order
	{
		public int TotalCount { get; set; }
		public int UniqueCount { get; set; }
		public string Weight { get; set; }
		public bool DriveThruSent { get; set; }
		public string ShippingMethod { get; set; }
		public string BuyerRealName { get; set; }
	}
}
