using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Data.Entities
{
	public class PartInventory : BaseEntity
	{
		public PartInventory()
		{
			LocationHistory = new List<PartInventoryLocationHistory>();
		}

		[Required]
		public virtual Part Part { get; set; }
		public virtual ICollection<PartInventoryLocationHistory> LocationHistory { get; set; }
		public virtual PartPriceInfo Pricing { get; set; }

		public int InventoryId { get; set; }
		public int Quantity { get; set; }
		public decimal MyPrice { get; set; }
		public int ColourId { get; set; }
		public string ColourName { get; set; }
		public char Condition { get; set; }
		public string Location { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public string Notes { get; set; }
		public DateTime LastUpdated { get; set; }
	}
}
