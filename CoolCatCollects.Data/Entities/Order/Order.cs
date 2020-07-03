using System;
using System.Collections.Generic;

namespace CoolCatCollects.Data.Entities
{
	public abstract class Order : BaseEntity
	{
		public string OrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public string BuyerName { get; set; }
		public string BuyerEmail { get; set; }
		public decimal Subtotal { get; set; }
		public decimal Shipping { get; set; }
		public decimal Deductions { get; set; }
		public decimal ExtraCosts { get; set; }
		public decimal GrandTotal { get; set; }
		public decimal Tax { get; set; }
		public OrderStatus Status { get; set; }
		public virtual ICollection<OrderItem> OrderItems { get; set; }
	}

	public enum OrderStatus
	{
		InProgress,
		Complete,
		Cancelled
	}
}
