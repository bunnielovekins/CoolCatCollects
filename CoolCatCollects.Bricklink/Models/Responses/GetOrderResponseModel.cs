using System;

namespace CoolCatCollects.Bricklink.Models
{
	public class GetOrderResponseModel : BricklinkResponse<BricklinkOrder>
	{
	}

	public class BricklinkOrder
	{
		public int order_id { get; set; }
		public DateTime date_ordered { get; set; }
		public DateTime date_status_changed { get; set; }
		public string seller_name { get; set; }
		public string store_name { get; set; }
		public string buyer_name { get; set; }
		public string buyer_email { get; set; }
		public bool require_insurance { get; set; }
		public string status { get; set; }
		public bool is_invoiced { get; set; }
		public int total_count { get; set; }
		public int unique_count { get; set; }
		public string total_weight { get; set; }
		public int buyer_order_count { get; set; }
		public bool is_filed { get; set; }
		public bool drive_thru_sent { get; set; }
		public BricklinkPayment payment { get; set; }
		public BricklinkShipping shipping { get; set; }
		public BricklinkCostFull cost { get; set; }
		public BricklinkDispCostFull disp_cost { get; set; }
	}

	public class BricklinkShipping
	{
		public int method_id { get; set; }
		public string method { get; set; }
		public BricklinkAddress address { get; set; }
	}

	public class BricklinkAddress
	{
		public BricklinkName name { get; set; }
		public string full { get; set; }
		public string address1 { get; set; }
		public string address2 { get; set; }
		public string country_code { get; set; }
		public string city { get; set; }
		public string state { get; set; }
		public string postal_code { get; set; }
	}

	public class BricklinkName
	{
		public string full { get; set; }
		public string first { get; set; }
		public string last { get; set; }
	}

	public class BricklinkCostFull
	{
		public string currency_code { get; set; }
		public string subtotal { get; set; }
		public string grand_total { get; set; }
		public string etc1 { get; set; }
		public string etc2 { get; set; }
		public string insurance { get; set; }
		public string shipping { get; set; }
		public string credit { get; set; }
		public string coupon { get; set; }
		public string salesTax { get; set; }
		public string vat_rate { get; set; }
		public string vat_amount { get; set; }
	}

	public class BricklinkDispCostFull
	{
		public string currency_code { get; set; }
		public string subtotal { get; set; }
		public string grand_total { get; set; }
		public string etc1 { get; set; }
		public string etc2 { get; set; }
		public string insurance { get; set; }
		public string shipping { get; set; }
		public string credit { get; set; }
		public string coupon { get; set; }
		public string vat_rate { get; set; }
		public string vat_amount { get; set; }
	}

}
