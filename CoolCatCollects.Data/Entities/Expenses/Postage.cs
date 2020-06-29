namespace CoolCatCollects.Data.Entities.Expenses
{
	public class Postage : BaseEntity
	{
		public decimal Price { get; set; }
		public string Service { get; set; }
		public virtual Order Order { get; set; }
	}
}
