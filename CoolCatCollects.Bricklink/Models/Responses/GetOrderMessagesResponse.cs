using System;
using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetOrderMessagesResponse : BricklinkResponse<IEnumerable<OrderMessage>>
	{

	}

	public class OrderMessage
	{
		public int messageID { get; set; }
		public string subject { get; set; }
		public string body { get; set; }
		public string from { get; set; }
		public string to { get; set; }
		public DateTime dateSent { get; set; }
	}

}
