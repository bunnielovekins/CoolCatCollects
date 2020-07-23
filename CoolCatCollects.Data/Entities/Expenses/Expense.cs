using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CoolCatCollects.Data.Entities.Expenses
{
	public class Expense : BaseEntity
	{
		public DateTime Date { get; set; }
		[DisplayName("Tax Category")]
		public string TaxCategory { get; set; }
		public string Category { get; set; }
		public string Source { get; set; }
		[DisplayName("Expenditure Type")]
		public string ExpenditureType { get; set; }
		[DisplayName("Order Number")]
		public string OrderNumber { get; set; }
		public decimal Price { get; set; }
		public decimal Postage { get; set; }
		public bool Receipt { get; set; }
		public string Notes { get; set; }
		public string Item { get; set; }
		public string Quantity { get; set; }
	}
}
