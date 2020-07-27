using System.Collections.Generic;

namespace CoolCatCollects.Models
{
	/// <summary>
	/// View model for the logs page
	/// </summary>
	public class LogListModel
	{
		public IEnumerable<LogModel> Items { get; set; }
		/// <summary>
		/// ASC or DESC
		/// </summary>
		public string Sort { get; set; }
		public bool SortAsc => Sort == "ASC";
		/// <summary>
		/// Opposite of what the current Sort is
		/// </summary>
		public string SortToggle => SortAsc ? "DESC" : "ASC";

		public string Category { get; set; }
		public string Search { get; set; }
	}
}