using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models
{
	public class GetOrderItemsResponseModel : BricklinkResponse<List<List<OrderItemResponseModel>>>
	{
	}

	public class OrderItemResponseModel
	{
		public int inventory_id { get; set; }
		public OrderItemItemModel item { get; set; }
		public int color_id { get; set; }
		public string color_name { get; set; }
		public int quantity { get; set; }
		public string new_or_used { get; set; }
		public string unit_price { get; set; }
		public string description { get; set; }
		public string remarks { get; set; }
		public string disp_unit_price { get; set; }
		public string disp_unit_price_final { get; set; }
		public string unit_price_final { get; set; }
		public string order_cost { get; set; }
		public string currency_code { get; set; }
		public string disp_currency_code { get; set; }
		public string weight { get; set; }
	}

	public class OrderItemItemModel
	{
		public string no { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public int category_id { get; set; }
	}

}
