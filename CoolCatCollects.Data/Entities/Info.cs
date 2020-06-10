using System;

namespace CoolCatCollects.Data.Entities
{
	public class Info : BaseEntity
	{
		public Info()
		{
			InventoryLastUpdated = new DateTime(1970, 1, 1);
		}

		public DateTime InventoryLastUpdated { get; set; }
	}
}
