using System.ComponentModel.DataAnnotations.Schema;

namespace CoolCatCollects.Data.Entities
{
	public class EbayOrderItem : OrderItem
	{
		public string LineItemId { get; set; }
		public string LegacyItemId { get; set; }
		public string LegacyVariationId { get; set; }
		public string SKU { get; set; }
		public string Image { get; set; }
		public string CharacterName { get; set; }
	}
}
