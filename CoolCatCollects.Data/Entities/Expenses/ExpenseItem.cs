using System.ComponentModel;

namespace CoolCatCollects.Data.Entities.Expenses
{
	public class ExpenseItem : BaseEntity
	{
		public string Description { get; set; }
		[DisplayName("Unit Price")]
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public virtual PartInventory Part { get; set; }
	}
}
