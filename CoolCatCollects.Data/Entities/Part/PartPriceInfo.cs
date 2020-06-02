using System;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Data.Entities
{
	public class PartPriceInfo : BaseEntity
	{
		[Required]
		public virtual PartInventory InventoryItem { get; set; }

		public decimal AveragePrice { get; set; }
		public string AveragePriceLocation { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
