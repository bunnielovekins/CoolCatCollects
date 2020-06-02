using System.Collections.Generic;

namespace CoolCatCollects.Ebay.Models.Responses
{
	public class GetOrdersResponseModel
	{
		public string href { get; set; }
		public int total { get; set; }
		public string next { get; set; }
		public int limit { get; set; }
		public int offset { get; set; }
		public IEnumerable<GetOrderResponseModel> orders { get; set; }
	}
}
