using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoolCatCollects.Data.Entities
{
	public class Part : BaseEntity
	{
		public Part()
		{
			LastUpdated = DateTime.Now;
			InventoryItems = new List<PartInventory>();
		}

		public string Number { get; set; }
		public string ItemType { get; set; }
		public string Name { get; set; }
		public int CategoryId { get; set; }
		public string ImageUrl { get; set; }
		public string ThumbnailUrl { get; set; }
		public string Weight { get; set; }
		public string Description { get; set; }

		public DateTime LastUpdated { get; set; }

		public virtual ICollection<PartInventory> InventoryItems { get; set; }

	}
}
