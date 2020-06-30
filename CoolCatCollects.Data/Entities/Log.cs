using System;

namespace CoolCatCollects.Data.Entities
{
	public class Log : BaseEntity
	{
		public DateTime Date { get; set; }
		public string Title { get; set; }
		public string Note { get; set; }
		public string FurtherNote { get; set; }
		public string Category { get; set; }
	}
}
