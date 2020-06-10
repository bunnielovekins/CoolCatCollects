using CoolCatCollects.Data.Entities;

namespace CoolCatCollects.Models
{
	public class HomepageModel
	{
		public HomepageModel()
		{

		}

		public HomepageModel(Info info)
		{
			InventoryLastUpdated = info.InventoryLastUpdated.Year <= 2010 ? "Never" : info.InventoryLastUpdated.ToShortDateString();
		}

		public string InventoryLastUpdated { get; set; }
	}
}