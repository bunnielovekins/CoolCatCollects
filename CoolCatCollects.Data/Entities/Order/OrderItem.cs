namespace CoolCatCollects.Data.Entities
{
	public abstract class OrderItem : BaseEntity
	{
		public string Name { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }


		public virtual PartInventory Part { get; set; }
		public virtual Order Order { get; set; }
	}
}
