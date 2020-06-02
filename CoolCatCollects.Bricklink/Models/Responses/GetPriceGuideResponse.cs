using System;

namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetPriceGuideResponse : BricklinkResponse<PriceGuideItem>
	{
	}

	public class PriceGuideItem
	{
		public Item item { get; set; }
		public string new_or_used { get; set; }
		public string currency_code { get; set; }
		public string min_price { get; set; }
		public string max_price { get; set; }
		public string avg_price { get; set; }
		public string qty_avg_price { get; set; }
		public int unit_quantity { get; set; }
		public int total_quantity { get; set; }
		public Price_Detail[] price_detail { get; set; }
	}

	public class Item
	{
		public string no { get; set; }
		public string type { get; set; }
	}

	public class Price_Detail
	{
		public int quantity { get; set; }
		public string unit_price { get; set; }
		public string seller_country_code { get; set; }
		public string buyer_country_code { get; set; }
		public DateTime date_ordered { get; set; }
		public int qunatity { get; set; }
	}

}
