using System.ComponentModel;

namespace CoolCatCollects.Models.Expenses
{
	public class ExpenseItemModel
	{
		public int Id { get; set; }
		public string Description { get; set; }
		[DisplayName("Unit Price")]
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		//public virtual PartInventory Part { get; set; }
	}
}
