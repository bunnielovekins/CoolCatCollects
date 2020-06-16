using System.ComponentModel.DataAnnotations.Schema;

namespace CoolCatCollects.Data.Entities
{
	public class EbayOrder : Order
	{
		public string LegacyOrderId { get; set; }
		public string SalesRecordReference { get; set; }
		public string BuyerUsername { get; set; }
	}
}
