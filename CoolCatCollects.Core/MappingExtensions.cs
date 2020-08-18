using CoolCatCollects.Data.Entities;
using CoolCatCollects.Models.Parts;

namespace CoolCatCollects.Core
{
	public static class MappingExtensions
	{
		public static PartInventory ToEntity(this PartInventoryModel model)
		{
			return new PartInventory
			{
				Id = model.Id,
				InventoryId = model.InventoryId,
				Quantity = model.Quantity,
				MyPrice = model.MyPrice,
				ColourId = model.ColourId,
				ColourName = model.ColourName,
				Condition = model.Condition,
				Location = model.Location,
				Image = model.Image,
				Description = model.Description,
				Notes = model.Notes,
				LastUpdated = model.LastUpdated
			};
		}

		public static void UpdateFromModel(this PartInventory inv, PartInventoryModel model)
		{
			inv.InventoryId = model.InventoryId;
			inv.Quantity = model.Quantity;
			inv.MyPrice = model.MyPrice;
			inv.ColourId = model.ColourId;
			inv.ColourName = model.ColourName;
			inv.Condition = model.Condition;
			inv.Location = model.Location;
			inv.Image = model.Image;
			inv.Description = model.Description;
			inv.Notes = model.Notes;
			inv.LastUpdated = model.LastUpdated;
		}
	}
}
