using System;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models
{
	public class GetOrdersResponseModel : BricklinkResponse<IEnumerable<GetOrdersResponseModelItem>>
	{

	}

	public class GetOrdersResponseModelItem
	{
		public int order_id { get; set; }
		public DateTime date_ordered { get; set; }
		public DateTime date_status_changed { get; set; }
		public string seller_name { get; set; }
		public string store_name { get; set; }
		public string buyer_name { get; set; }
		public string status { get; set; }
		public int total_count { get; set; }
		public int unique_count { get; set; }
		public bool is_filed { get; set; }
		public BricklinkPayment payment { get; set; }
		public BricklinkCost cost { get; set; }
		public BricklinkCost disp_cost { get; set; }
	}

	public class BricklinkPayment
	{
		public string method { get; set; }
		public string currency_code { get; set; }
		public DateTime date_paid { get; set; }
		public string status { get; set; }
	}

	public class BricklinkCost
	{
		public string currency_code { get; set; }
		public string subtotal { get; set; }
		public string grand_total { get; set; }
	}

}
