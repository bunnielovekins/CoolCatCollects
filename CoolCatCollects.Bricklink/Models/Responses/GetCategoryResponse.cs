namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetCategoryResponse : BricklinkResponse<GetCategoryResponseData>
	{
	}

	public class GetCategoryResponseData
	{
		public int category_id { get; set; }
		public string category_name { get; set; }
		public int parent_id { get; set; }
	}

}
