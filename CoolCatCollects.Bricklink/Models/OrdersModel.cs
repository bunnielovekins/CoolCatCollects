using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink.Models
{
	public class OrdersModel
	{
		public OrdersModel()
		{

		}

		public OrdersModel(GetOrdersResponseModel model, string status)
		{
			Orders = model.data.Select(x => new OrderModel(x)).ToList();
			Status = status;
		}

		public List<OrderModel> Orders { get; set; }
		public string Status { get; set; }
	}

	public class OrderModel
	{
		public bool Selected { get; set; }
		public int OrderId { get; set; }
		public int TotalPieces { get; set; }
		public int UniquePieces { get; set; }
		public string BuyerName { get; set; }
		public string Total { get; set; }

		public OrderModel()
		{

		}

		public OrderModel(GetOrdersResponseModelItem model)
		{
			Selected = true;
			OrderId = model.order_id;
			TotalPieces = model.total_count;
			UniquePieces = model.unique_count;
			BuyerName = model.buyer_name;
			Total = model.cost.grand_total;
		}
	}
}
