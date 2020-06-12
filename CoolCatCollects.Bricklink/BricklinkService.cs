using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

		public object GetOrdersNotInDb()
		{
			var result = _apiService.GetRequest($"orders?direction=in&status=shipped,received,completed");

			var responseModel = JsonConvert.DeserializeObject<GetOrdersResponseModel>(result);

			var dbOrders = _dataService.GetOrderIds().ToList();

			var items = responseModel.data.Where(x => !dbOrders.Contains(x.order_id.ToString()));

			var model = new
			{
				count = items.Count(),
				items = items
					.Select(x => new
					{
						id = x.order_id
					})
			};

			return model;
		}

		public IEnumerable<string> UpdateInventoryForColour(int colourId)
		{
			var result = _apiService.GetRequest<GetInventoriesResponseModel>("inventories?color_id=" + colourId);
			var errors = new List<string>();

			foreach(BricklinkInventoryItemModel inv in result.data)
			{
				try
				{
					_dataService.GetPartModel(inv.item.no, inv.color_id, inv.item.type, inv.new_or_used[0], true);
				}
				catch(Exception ex)
				{
					errors.Add(ex.Message);
				}
			}

			return errors;
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

		public OrderCsvModel GetOrderForCsv(string orderId)
		{
			var response = GetOrder(orderId);

			var model = new OrderCsvModel(response);

			return model;
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

			var orderEntity = _dataService.AddOrder(order, orderItems);

			var items = orderItems.data
				.SelectMany(x => x)
				.Select(item =>
				{
					var itemEntity = orderEntity.OrderItems.FirstOrDefault(x => 
						item.item.no == x.Part.Part.Number && 
						item.color_id == x.Part.ColourId &&
						item.item.type == x.Part.Part.ItemType &&
						item.new_or_used == x.Part.Condition);

					var unitPrice = decimal.Parse(item.unit_price_final);
					var totalPrice = unitPrice * item.quantity;

					var itemModel = new OrderItemModel
					{
						InventoryId = item.inventory_id.ToString(),
						Name = HttpUtility.HtmlDecode(item.item.name),
						Condition = item.new_or_used == "N" ? "New" : "Used",
						Colour = itemEntity.Part.ColourName,
						Remarks = item.remarks,
						Quantity = item.quantity,
						UnitPrice = unitPrice,
						TotalPrice = totalPrice,
						Description = item.description,
						Type = itemEntity.Part.Part.ItemType,
						Weight = itemEntity.Part.Part.Weight,
						ItemsRemaining = itemEntity.Part.Quantity,
						Image = itemEntity.Part.Image
					};

					if (order.data.date_ordered > itemEntity.Part.LastUpdated)
					{
						_dataService.UpdatePartInventoryFromOrder(itemEntity.Part, item.remarks, item.unit_price_final, item.description, item.inventory_id);
					}

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
				BuyerName = orderEntity.BuyerName,
				UserName = data.buyer_name,
				ShippingMethod = orderEntity.ShippingMethod,
				OrderTotal = orderEntity.Subtotal.ToString(),
				Buyer = new Buyer(data.shipping.address),
				OrderNumber = orderEntity.OrderId,
				OrderDate = orderEntity.OrderDate.ToString("yyyy-MM-dd"),
				OrderPaid = data.payment.date_paid.ToString("yyyy-MM-dd"),
				OrderRemarks = data.remarks,
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
