using System;
using System.Collections;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetInventoriesResponseModel : BricklinkResponse<IEnumerable<BricklinkInventoryItemModel>>
	{
	}

	public class GetInventoryResponseModel : BricklinkResponse<BricklinkInventoryItemModel>
	{
	}

	public class BricklinkInventoryItemModel
	{
		public int inventory_id { get; set; }
		public BricklinkInventoryItemItemModel item { get; set; }
		public int color_id { get; set; }
		public string color_name { get; set; }
		public int quantity { get; set; }
		public string new_or_used { get; set; }
		public string unit_price { get; set; }
		public int bind_id { get; set; }
		public string description { get; set; }
		public string remarks { get; set; }
		public int bulk { get; set; }
		public bool is_retain { get; set; }
		public bool is_stock_room { get; set; }
		public DateTime date_created { get; set; }
		public string my_cost { get; set; }
		public int sale_rate { get; set; }
		public int tier_quantity1 { get; set; }
		public string tier_price1 { get; set; }
		public int tier_quantity2 { get; set; }
		public string tier_price2 { get; set; }
		public int tier_quantity3 { get; set; }
		public string tier_price3 { get; set; }
		public string my_weight { get; set; }
	}

	public class BricklinkInventoryItemItemModel
	{
		public string no { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public int category_id { get; set; }
	}

}
