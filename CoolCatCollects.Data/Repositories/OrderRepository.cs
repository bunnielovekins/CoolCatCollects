using CoolCatCollects.Data.Entities;
using System.Collections.Generic;

namespace CoolCatCollects.Data.Repositories
{
	public class OrderRepository : BaseRepository<Order>
	{
		public OrderRepository() : base()
		{

		}

		public OrderRepository(EfContext context) : base(context)
		{

		}

		public BricklinkOrder AddOrderWithItems(BricklinkOrder orderEntity, List<BricklinkOrderItem> orderItemEntities)
		{
			orderEntity = _ctx.BricklinkOrders.Add(orderEntity);

			foreach(var item in orderItemEntities)
			{
				item.Part = _ctx.PartInventorys.Attach(item.Part);

				_ctx.BricklinkOrderItems.Add(item);
			}

			_ctx.SaveChanges();

			return orderEntity;
		}
	}
}
