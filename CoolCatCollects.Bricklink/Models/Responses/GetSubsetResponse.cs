using System.Collections.Generic;

namespace CoolCatCollects.Bricklink.Models.Responses
{
	public class GetSubsetResponse : BricklinkResponse<IEnumerable<BricklinkSubsetPart>>
	{
	}

	public class BricklinkSubsetPart
	{
		public int match_no { get; set; }
		public BricklinkSubsetPartEntry[] entries { get; set; }
	}

	public class BricklinkSubsetPartEntry
	{
		public BricklinkSubsetPartItem item { get; set; }
		public int color_id { get; set; }
		public int quantity { get; set; }
		public int extra_quantity { get; set; }
		public bool is_alternate { get; set; }
		public bool is_counterpart { get; set; }
	}

	public class BricklinkSubsetPartItem
	{
		public string no { get; set; }
		public string name { get; set; }
		public string type { get; set; }
		public int category_id { get; set; }
	}

}
