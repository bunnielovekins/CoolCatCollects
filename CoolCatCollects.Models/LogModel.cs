using System;
using System.ComponentModel.DataAnnotations;

namespace CoolCatCollects.Models
{
	public class LogModel
	{
		public LogModel()
		{
			Date = DateTime.Now;
		}

		public int Id { get; set; }
		public DateTime Date { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		[DataType(DataType.MultilineText)]
		public string Note { get; set; }
		[Required]
		public string Category { get; set; }
	}
}
