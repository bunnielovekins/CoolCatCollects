using CoolCatCollects.Bricklink;
using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Core;
using CoolCatCollects.Data.Entities;
using System.Collections.Generic;

namespace CoolCatCollects.Models
{
	public class DatabaseUpdateModel
	{
		public DatabaseUpdateModel()
		{

		}

		public DatabaseUpdateModel(Info info)
		{
			InventoryLastUpdated = info.InventoryLastUpdated.Year <= 2010 ? "Never" : info.InventoryLastUpdated.ToShortDateString();

			Colours = new ColourService().GetAll();
		}

		public string InventoryLastUpdated { get; set; }

		public IEnumerable<ColourModel> Colours { get; set; }
	}
}