using CoolCatCollects.Data.Entities;

namespace CoolCatCollects.Bricklink.Models
{
	public class PartModel
	{
		public Part Part { get; set; }
		public PartInventory PartInventory { get; set; }
		public PartPriceInfo PartPriceInfo { get; set; }

		public bool InvIsPlaceHolder { get; set; }
	}
}
