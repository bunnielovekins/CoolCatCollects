namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetItemResponse : BricklinkResponse<BricklinkItem>
	{
	}

	public class BricklinkItem
	{
		public string no { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public int category_id { get; set; }
		public string image_url { get; set; }
		public string thumbnail_url { get; set; }
		public string weight { get; set; }
		public string dim_x { get; set; }
		public string dim_y { get; set; }
		public string dim_z { get; set; }
		public int year_released { get; set; }
		public string description { get; set; }
		public bool is_obsolete { get; set; }
	}
}
