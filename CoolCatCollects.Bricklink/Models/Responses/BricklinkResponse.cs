using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models
{
	public class BricklinkResponse<T>
	{
		public BricklinkMeta meta { get; set; }
		public T data { get; set; }
	}

	public class BricklinkMeta
	{
		public string description { get; set; }
		public string message { get; set; }
		public int code { get; set; }
	}
}
