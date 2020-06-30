using System.Collections.Generic;

namespace CoolCatCollects.Models
{
	public class LogListModel
	{
		public IEnumerable<LogModel> Items { get; set; }
		public string Sort { get; set; }
		public bool SortAsc => Sort == "ASC";
		public string SortToggle => SortAsc ? "DESC" : "ASC";

		public string Category { get; set; }
		public string Search { get; set; }
	}
}