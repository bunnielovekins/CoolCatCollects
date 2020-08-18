using CoolCatCollects.Bricklink.Models;
using System.Collections.Generic;

namespace CoolCatCollects.Models
{
	public class LocationHistoryModel : List<PartModel>
	{
		public string Location { get; set; }
	}
}