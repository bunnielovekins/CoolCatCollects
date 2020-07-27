using System.Collections.Generic;
using static CoolCatCollects.Core.PostageCalculator;

namespace CoolCatCollects.Models
{
	/// <summary>
	/// View model for the postage calculator
	/// </summary>
	public class PostageModel
	{
		public PostageResult Economy { get; set; }
		public PostageResult Standard { get; set; }
		public PostageResult Express { get; set; }
		public IEnumerable<PostageResult> Alternatives { get; set; }

		public string Price { get; set; }
		public string Weight { get; set; }
		public string Size { get; set; }
	}
}