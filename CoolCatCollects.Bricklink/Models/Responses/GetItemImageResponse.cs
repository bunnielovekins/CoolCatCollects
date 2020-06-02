namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetItemImageResponse : BricklinkResponse<GetItemImageResponseData>
	{
	}

	public class GetItemImageResponseData
	{
		public string no { get; set; }
		public string type { get; set; }
		public string thumbnail_url { get; set; }
	}

}
