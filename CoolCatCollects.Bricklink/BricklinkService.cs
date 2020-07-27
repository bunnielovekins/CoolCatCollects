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
	/// <summary>
	/// Top-level class that deals with BL
	/// </summary>
	public class BricklinkService
	{
		private readonly BricklinkApiService _apiService;
		private readonly BricklinkDataService _dataService;

		public BricklinkService()
		{
			_apiService = new BricklinkApiService();
			_dataService = new BricklinkDataService();
		}

		/// <summary>
		/// Gets a list of orders of a status. No order details
		/// </summary>
		/// <param name="status">Status to search for</param>
		/// <returns>Model - list of orders, also status</returns>
		public OrdersModel GetOrders(string status)
		{
			var result = _apiService.GetRequest($"orders?direction=in&status={status.ToUpper()}");

			var responseModel = JsonConvert.DeserializeObject<GetOrdersResponseModel>(result);

			var model = new OrdersModel(responseModel, status);

			return model;
		}

		/// <summary>
		/// Gets orders that aren't in the DB
		/// </summary>
		/// <returns>A Json object</returns>
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

		/// <summary>
		/// Gets messages for an order. Ignores feedback messages.
		/// </summary>
		/// <param name="orderId">Order Id</param>
		/// <returns>BL Message Model</returns>
		public IEnumerable<BricklinkMessage> GetOrderMessages(string orderId)
		{
			var result = _apiService.GetRequest<GetOrderMessagesResponse>($"orders/{orderId}/messages");

			return result.data.Select(x => new BricklinkMessage
			{
				InOrOut = x.from == "mroseives" ? "Out" : "In",
				Subject = x.subject,
				Body = x.body,
				Date = x.dateSent
			}).Where(x => 
				x.Body != "You left seller feedback." && 
				x.Body != "Seller left you feedback." &&
				!(x.Subject ?? "").Contains("Invoice for BrickLink Order")
			);
		}

		/// <summary>
		/// Updates DB inventory for a colour by calling GetPartModel. Used only in DB update force.
		/// </summary>
		/// <param name="colourId">Colour ID</param>
		/// <returns>List of errors</returns>
		public IEnumerable<string> UpdateInventoryForColour(int colourId)
		{
			var result = _apiService.GetRequest<GetInventoriesResponseModel>("inventories?color_id=" + colourId);
			var errors = new List<string>();

			foreach(BricklinkInventoryItemModel inv in result.data)
			{
				try
				{
					_dataService.GetPartModel(inv.item.no, inv.color_id, inv.item.type, inv.new_or_used, true);
				}
				catch(Exception ex)
				{
					errors.Add(ex.Message);
				}
			}

			return errors;
		}

		/// <summary>
		/// Gets an order in a format for the CSV export
		/// </summary>
		/// <param name="orderId">Order Id</param>
		/// <returns>OrderCsvModel for export</returns>
		public OrderCsvModel GetOrderForCsv(string orderId)
		{
			var response = _apiService.GetRequest<GetOrderResponseModel>("orders/" + orderId);

			var model = new OrderCsvModel(response);

			return model;
		}

		/// <summary>
		/// Gets an order with its order items
		/// </summary>
		/// <param name="orderId">Order Id</param>
		/// <returns>Model with order and items</returns>
		public OrderWithItemsModel GetOrderWithItems(string orderId)
		{
			var order = _apiService.GetRequest<GetOrderResponseModel>("orders/" + orderId);

			var orderItems = _apiService.GetRequest<GetOrderItemsResponseModel>("orders/" + orderId + "/items");

			// Add order to the DB
			var orderEntity = _dataService.AddOrder(order, orderItems);

			// Items come through in a very strange format, so get them from the DB after having just added them to the DB.
			var items = orderItems.data
				.SelectMany(x => x)
				.Select(item =>
				{
					// Find this item in the DB
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
						Type = item.item.type,
						Weight = item.weight,
						ItemsRemaining = itemEntity.Part.Quantity,
						Image = itemEntity.Part.Image
					};

					if (order.data.date_ordered > itemEntity.Part.LastUpdated)
					{
						// Update the inventory from the info we have in this order
						_dataService.UpdatePartInventoryFromOrder(itemEntity.Part, item.remarks, item.unit_price_final, item.description, item.inventory_id);
					}

					// Works out some stuff related to the remarks and the location ordering
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

		/// <summary>
		/// Gets all the parts in a set
		/// </summary>
		/// <param name="set">Set number. Should already be checked to include -1</param>
		/// <returns>A list of parts</returns>
		public SubsetPartsListModel GetPartsFromSet(string set)
		{
			var responseModel = _apiService.GetRequest<GetSubsetResponse>($"items/SET/{set}/subsets");

			var model = new SubsetPartsListModel(responseModel);

			model.Parts = model.Parts
				.Select(x =>
				{
					var part = _dataService.GetPartModel(x.Number, x.ColourId, x.Type, "N", true);

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

		/// <summary>
		/// Gets details of a set; used for stock purchases. Mostly to do with value.
		/// </summary>
		/// <param name="set">Set number. -1 should already be there.</param>
		/// <returns>Object for json</returns>
		public object GetSetDetails(string set)
		{
			var setInfo = _apiService.GetRequest<GetItemResponse>($"items/SET/{set}");

			var categoryInfo = _apiService.GetRequest<GetCategoryResponse>($"categories/{setInfo.data.category_id}");

			var parts = _apiService.GetRequest<GetSubsetResponse>($"items/SET/{set}/subsets");

			var minifigs = parts.data.SelectMany(x => x.entries).Where(x => x.item.type == "MINIFIG").Select(x => _dataService.GetPartModel(x.item.no, x.color_id, x.item.type, "N"));

			var minifigParts = minifigs.Select(x => _apiService.GetRequest<GetSubsetResponse>($"items/MINIFIG/{set}/subsets")).
				SelectMany(x => x.data).
				SelectMany(x => x.entries).
				Select(x => new { model = _dataService.GetPartModel(x.item.no, x.color_id, x.item.type, "N"), quantity = x.quantity + x.extra_quantity }).ToList();

			var allParts = parts.data.SelectMany(x => x.entries).Where(x => x.item.type == "PART").
				Select(x => new { model = _dataService.GetPartModel(x.item.no, x.color_id, x.item.type, "N"), quantity = x.quantity + x.extra_quantity });

			var minifigValue = minifigParts.Sum(x => x.model.PartPriceInfo.AveragePrice * x.quantity);
			var partsValue = allParts.Sum(x => x.model.PartPriceInfo.AveragePrice * x.quantity);

			return new
			{
				SetName = HttpUtility.HtmlDecode(setInfo.data.name),
				Theme = HttpUtility.HtmlDecode(categoryInfo.data.category_name),
				Image = setInfo.data.image_url,
				Parts = parts.data.SelectMany(x => x.entries).Where(x => x.item.type == "PART").Sum(x => x.quantity),
				MinifigValue = minifigValue,
				PartsValue = partsValue
			};
		}
	}
}
