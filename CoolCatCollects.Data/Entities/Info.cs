using System;

namespace CoolCatCollects.Data.Entities
{
	public class Info : BaseEntity
	{
		public Info()
		{
			InventoryLastUpdated = new DateTime(1970, 1, 1);
			OrdersLastUpdated = new DateTime(1970, 1, 1);
		}

		public DateTime InventoryLastUpdated { get; set; }
		public DateTime OrdersLastUpdated { get; set; }
	}
}
