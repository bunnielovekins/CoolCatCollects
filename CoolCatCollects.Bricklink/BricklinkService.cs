using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web;

namespace CoolCatCollects.Bricklink
{
	public class BricklinkService
	{
		private readonly BricklinkApiService _apiService;
		private readonly BricklinkDataService _dataService;

		public BricklinkService()
		{
			_apiService = new BricklinkApiService();
			_dataService = new BricklinkDataService();
		}

		public OrdersModel GetOrders(string status)
		{
			var result = _apiService.GetRequest($"orders?direction=in&status={status.ToUpper()}");

			var responseModel = JsonConvert.DeserializeObject<GetOrdersResponseModel>(result);

			var model = new OrdersModel(responseModel, status);

			return model;
		}

		public OrderCsvModel GetOrderForCsv(string orderId)
		{
			var result = _apiService.GetRequest("orders/" + orderId);

			var responseModel = JsonConvert.DeserializeObject<GetOrderResponseModel>(result);

			if (responseModel.data == null)
			{
				result = _apiService.GetRequest("orders/" + orderId);

				responseModel = JsonConvert.DeserializeObject<GetOrderResponseModel>(result);

				if (responseModel.data == null)
				{
					throw new Exception("An error has occurred! Order ID: " + orderId + ", Error code: " + responseModel.meta.code +
						", Error Message: " + responseModel.meta.message +
						", Error Description: " + responseModel.meta.description);
				}
			}

			var model = new OrderCsvModel(responseModel);

			return model;
		}

		public int GetItemsRemaining(string inventoryId)
		{
			var result = _apiService.GetRequest("/inventories/" + inventoryId);

			var responseModel = JsonConvert.DeserializeObject<GetInventoryResponseModel>(result);

			if (responseModel.data == null)
			{
				result = _apiService.GetRequest("/inventories/" + inventoryId);

				responseModel = JsonConvert.DeserializeObject<GetInventoryResponseModel>(result);

				if (responseModel.data == null)
				{
					throw new Exception("An error has occurred! inventoryId: " + inventoryId + ", Error code: " + responseModel.meta.code +
						", Error Message: " + responseModel.meta.message +
						", Error Description: " + responseModel.meta.description);
				}
			}

			return responseModel.data.quantity;
		}

		public GetOrderResponseModel GetOrder(string orderId)
		{
			var result = _apiService.GetRequest("orders/" + orderId);

			var responseModel = JsonConvert.DeserializeObject<GetOrderResponseModel>(result);

			if (responseModel.data == null)
			{
				result = _apiService.GetRequest("orders/" + orderId);

				responseModel = JsonConvert.DeserializeObject<GetOrderResponseModel>(result);

				if (responseModel.data == null)
				{
					throw new Exception("An error has occurred! Order ID: " + orderId + ", Error code: " + responseModel.meta.code +
						", Error Message: " + responseModel.meta.message +
						", Error Description: " + responseModel.meta.description);
				}
			}

			return responseModel;
		}

		public GetOrderItemsResponseModel GetOrderItems(string orderId)
		{
			var result = _apiService.GetRequest("orders/" + orderId + "/items");

			var responseModel = JsonConvert.DeserializeObject<GetOrderItemsResponseModel>(result);

			if (responseModel.data == null)
			{
				result = _apiService.GetRequest("orders/" + orderId + "/items");

				responseModel = JsonConvert.DeserializeObject<GetOrderItemsResponseModel>(result);

				if (responseModel.data == null)
				{
					throw new Exception("An error has occurred! Order ID: " + orderId + ", Error code: " + responseModel.meta.code +
						", Error Message: " + responseModel.meta.message +
						", Error Description: " + responseModel.meta.description);
				}
			}

			return responseModel;
		}

		public OrderWithItemsModel GetOrderWithItems(string orderId)
		{
			var order = GetOrder(orderId);

			var orderItems = GetOrderItems(orderId);
			var items = orderItems.data
				.SelectMany(x => x)
				.Select(item =>
				{
					var inv = _dataService.GetPartModel(item.item.no, item.color_id, item.item.type, item.new_or_used[0], updateInvDate: order.data.date_ordered);

					var unitPrice = decimal.Parse(item.unit_price_final);
					var totalPrice = unitPrice * item.quantity;

					var itemModel = new OrderItemModel
					{
						InventoryId = item.inventory_id.ToString(),
						Name = HttpUtility.HtmlDecode(item.item.name),
						Condition = item.new_or_used == "N" ? "New" : "Used",
						Colour = inv.PartInventory.ColourName,
						Remarks = item.remarks,
						Quantity = item.quantity,
						UnitPrice = unitPrice,
						TotalPrice = totalPrice,
						Description = inv.PartInventory.Description,
						Type = item.item.type,
						Weight = item.weight,
						ItemsRemaining = inv.PartInventory.Quantity,
						Image = inv.PartInventory.Image
					};

					itemModel.FillRemarks();

					return itemModel;
				})
				.OrderBy(x => x.Condition)
				.ThenBy(x => x.RemarkLetter2)
				.ThenBy(x => x.RemarkLetter1)
				.ThenBy(x => x.RemarkNumber)
				.ThenBy(x => x.Colour)
				.ThenBy(x => x.Name);

			var data = order.data;

			return new OrderWithItemsModel
			{
				BuyerName = data.shipping.address.name.full,
				UserName = data.buyer_name,
				ShippingMethod = data.shipping.method,
				OrderTotal = data.cost.subtotal,
				Buyer = new Buyer(data.shipping.address),
				OrderNumber = data.order_id.ToString(),
				OrderDate = data.date_ordered.ToString("yyyy-MM-dd"),
				OrderPaid = data.payment.date_paid.ToString("yyyy-MM-dd"),
				SubTotal = StaticFunctions.FormatCurrencyStr(data.cost.subtotal),
				ServiceCharge = StaticFunctions.FormatCurrencyStr(data.cost.etc1),
				Coupon = StaticFunctions.FormatCurrencyStr(data.cost.coupon),
				PostagePackaging = StaticFunctions.FormatCurrencyStr(data.cost.shipping),
				Total = StaticFunctions.FormatCurrencyStr(data.cost.grand_total),
				Items = items
			};
		}

		public SubsetPartsListModel GetPartsFromSet(string set)
		{
			var result = _apiService.GetRequest("items/SET/" + set + "/subsets");

			var responseModel = JsonConvert.DeserializeObject<GetSubsetResponse>(result);

			if (responseModel.data == null)
			{
				result = _apiService.GetRequest("items/SET/" + set + "/subsets");

				responseModel = JsonConvert.DeserializeObject<GetSubsetResponse>(result);

				if (responseModel.data == null)
				{
					throw new Exception("An error has occurred! Set ID: " + set + ", Error code: " + responseModel.meta.code +
						", Error Message: " + responseModel.meta.message +
						", Error Description: " + responseModel.meta.description);
				}
			}

			var parts = responseModel.data.SelectMany(x => x.entries).Take(1).Select(x => _dataService.GetPartModel(x.item.no, x.color_id, x.item.type, 'N')).ToList();

			var model = new SubsetPartsListModel(responseModel);

			model.Parts = model.Parts
				.Select(x =>
				{
					var part = _dataService.GetPartModel(x.Number, x.ColourId, x.Type, 'A');

					x.MyPrice = part.PartInventory.MyPrice.ToString();
					x.Remark = part.PartInventory.Location;
					x.AveragePrice = part.PartPriceInfo.AveragePrice +
						(string.IsNullOrEmpty(part.PartPriceInfo.AveragePriceLocation) ? "" : " " + part.PartPriceInfo.AveragePriceLocation);

					if (part.PartInventory.ColourId != 0)
					{
						x.Colour = Statics.Colours[part.PartInventory.ColourId];
						x.ColourName = x.Colour.Name;
					}

					return x;
				})
				.OrderBy(x => x.Status)
				.ThenBy(x => x.Colour.Name)
				.ThenBy(x => x.Number);

			return model;
		}
	}
}
