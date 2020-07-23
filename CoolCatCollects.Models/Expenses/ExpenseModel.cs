using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Models.Expenses
{
	public class ExpenseModel
	{
		public int Id { get; set; }
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
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
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }
		public string Item { get; set; }
		public string Quantity { get; set; }
	}
}
