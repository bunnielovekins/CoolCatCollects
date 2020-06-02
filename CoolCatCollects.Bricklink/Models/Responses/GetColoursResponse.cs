using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models.Responses
{
	class GetColoursResponse : BricklinkResponse<IEnumerable<BricklinkColour>>
	{
	}

	public class BricklinkColour
	{
		public int color_id { get; set; }
		public string color_name { get; set; }
		public string color_code { get; set; }
		public string color_type { get; set; }
	}

}
