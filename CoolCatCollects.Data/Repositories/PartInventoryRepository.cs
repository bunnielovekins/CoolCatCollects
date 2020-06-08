using CoolCatCollects.Data.Entities;
using System;
using System.Data.Entity;
using System.Linq;

namespace CoolCatCollects.Data.Repositories
{
	public class PartInventoryRepository : BaseRepository<PartInventory>
	{
		public void AddPartInv(ref PartInventory inv, ref PartPriceInfo price, ref Part part)
		{
			if (part.Id == 0)
			{
				part = _ctx.Parts.Add(part);
			}
			else
			{
				int partId = part.Id;
				var local = _ctx.Set<Part>().Local.FirstOrDefault(x => x.Id == partId);
				if (local != null)
				{
					_ctx.Entry(local).State = EntityState.Detached;
				}
				part = _ctx.Parts.Attach(part);
			}

			inv.Part = part;

			inv = _ctx.PartInventorys.Add(inv);

			price.InventoryItem = inv;
			price = _ctx.PartPriceInfos.Add(price);

			if (inv.Location != "")
			{
				var history = new PartInventoryLocationHistory
				{
					Location = inv.Location,
					Date = DateTime.Now,
					PartInventory = inv
				};
				_ctx.PartInventoryLocationHistorys.Add(history);
			}

			_ctx.SaveChanges();
		}

		public override PartInventory Update(PartInventory entity)
		{
			entity = base.Update(entity);

			if (!string.IsNullOrEmpty(entity.Location) && (entity.LocationHistory == null || !entity.LocationHistory.Any(x => x.Location == entity.Location)))
			{
				entity = _ctx.PartInventorys.Attach(entity);

				_ctx.PartInventoryLocationHistorys.Add(new PartInventoryLocationHistory
				{
					Date = DateTime.Now,
					Location = entity.Location,
					PartInventory = entity
				});

				_ctx.SaveChanges();
			}

			return entity;
		}
	}
}
