using System;

namespace CoolCatCollects.Models.Parts
{
	public class PartInventoryModel
	{
		public PartInventoryModel()
		{
			LastUpdated = DateTime.Now;
		}

		public int Id { get; set; }
		public int InventoryId { get; set; }
		public int Quantity { get; set; }
		public decimal MyPrice { get; set; }
		public int ColourId { get; set; }
		public string ColourName { get; set; }
		public string Condition { get; set; }
		public string Location { get; set; }
		public string Image { get; set; }
		public string Description { get; set; }
		public string Notes { get; set; }
		public DateTime LastUpdated { get; set; }
		public string Number { get; set; }
		public string Name { get; set; }
		public string ItemType { get; set; }
		public int CategoryId { get; set; }
	}
}
