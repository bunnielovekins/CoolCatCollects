
using System;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Data.Entities
{
	public class PartInventoryLocationHistory : BaseEntity
	{
		public DateTime Date { get; set; }
		public string Location { get; set; }

		[Required]
		public virtual PartInventory PartInventory { get; set; }
	}
}
