using CoolCatCollects.Bricklink.Models;
using CoolCatCollects.Bricklink.Models.Responses;
using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Bricklink
{
	/// <summary>
	/// Service to deal with BL-related DB tables
	/// </summary>
	public class BricklinkDataService
	{
		private readonly BaseRepository<Part> _partrepo;
		private readonly PartInventoryRepository _partInventoryRepo;
		private readonly BaseRepository<PartPriceInfo> _partPricingRepo;
		private readonly OrderRepository _orderRepo;
		private readonly BricklinkApiService _api;

		public BricklinkDataService()
		{
			_partInventoryRepo = new PartInventoryRepository();
			_partrepo = new BaseRepository<Part>(_partInventoryRepo.Context);
			_partPricingRepo = new BaseRepository<PartPriceInfo>(_partInventoryRepo.Context);

			_orderRepo = new OrderRepository(_partInventoryRepo.Context);

			_api = new BricklinkApiService();
		}

		/// <summary>
		/// Gets a list of all order ids in the database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> GetOrderIds()
		{
			return _orderRepo.FindAll().Select(x => x.OrderId);
		}

		/// <summary>
		/// Main function, gets a Part/Inventory/Price model for a particular part number/colour/type/condition combination. If it doesn't exist, gets it from the api and creates it.
		/// </summary>
		/// <param name="number">Part Number</param>
		/// <param name="colourId">Colour Id</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <param name="condition">Condition - N, U, A. A = any, prefers new and falls back to used</param>
		/// <param name="updateInv">Forces an update of the inventory item from the API. By default, it updates once a day</param>
		/// <param name="updatePrice">Forces an update of the price from the API. By default, it updates once every 14 days</param>
		/// <param name="updatePart">Forces an update of the part from the API. By default, it updates once every 14 days</param>
		/// <param name="updateInvDate">Forces an update of the inventory item if it's older than this</param>
		/// <returns>A model with an attached Part, PartInventory, PartPriceInfo</returns>
		public PartModel GetPartModel(string number, int colourId, string type, string condition = "N",
			bool updateInv = false, bool updatePrice = false, bool updatePart = false,
			DateTime? updateInvDate = null)
		{
			string cond = condition == "A" ? "N" : condition;
			var partInv = _partInventoryRepo.FindOne(x => 
				x.Part.ItemType == type && 
				x.Part.Number == number && 
				x.ColourId == colourId && 
				x.Condition == cond);

			// No part inventory - create it
			if (partInv == null)
			{
				var part = GetPart(number, type);

				// Attempt to get the part from the API
				partInv = UpdateInventoryFromApi(type, part.CategoryId, colourId, number, condition == "A" ? "N" : condition);

				// Quantity is 0, fall back to used if condition is set to any
				if (partInv.Quantity == 0 && condition == "A")
				{
					var usedPart = UpdateInventoryFromApi(type, part.CategoryId, colourId, number, "U");
					if (usedPart.Quantity > 0)
					{
						partInv = usedPart;
					}
				}

				if (partInv.ColourId != 0)
				{
					// Lookup colour
					partInv.ColourName = Statics.Colours[partInv.ColourId].Name;
				}

				var pricing = UpdatePartPricingFromApi(number, type, colourId, partInv.Condition);

				_partInventoryRepo.AddPartInv(ref partInv, ref pricing, ref part);

				return new PartModel
				{
					Part = part,
					PartInventory = partInv,
					PartPriceInfo = pricing
				};
			}
			else
			{
				// Get the entities, make sure they're up to date, return them
				if (partInv.LastUpdated < DateTime.Now.AddDays(-1) || updateInv || updateInvDate.HasValue && updateInvDate > partInv.LastUpdated)
				{
					partInv = UpdateInventoryFromApi(partInv);
					_partInventoryRepo.Update(partInv);
				}

				var part = partInv.Part;
				if (part.LastUpdated < DateTime.Now.AddDays(-14) || updatePart)
				{
					part = UpdatePartFromApi(number, type, part);
					_partrepo.Update(part);
				}

				var pricing = partInv.Pricing;
				if (pricing.LastUpdated < DateTime.Now.AddDays(-14) || updatePrice)
				{
					pricing = UpdatePartPricingFromApi(number, type, colourId, condition, pricing);
					_partPricingRepo.Update(pricing);
				}

				return new PartModel
				{
					Part = part,
					PartInventory = partInv,
					PartPriceInfo = pricing
				};
			}
		}

		/// <summary>
		/// Gets the image url for a part
		/// </summary>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <param name="number">Part Number</param>
		/// <param name="colourId">Colour Id</param>
		/// <returns>Image url</returns>
		private string GetItemImage(string type, string number, int colourId)
		{
			var response = _api.GetRequest<GetItemImageResponse>($"/items/{type}/{number}/images/{colourId}");

			return response.data.thumbnail_url;
		}

		/// <summary>
		/// Gets a part from the DB. If it's out of date or not there, gets it from the API.
		/// </summary>
		/// <param name="number">Part Number</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <returns>A Part entity</returns>
		private Part GetPart(string number, string type)
		{
			var part = _partrepo.FindOne(x => x.ItemType == type && x.Number == number);

			if (part != null && part.LastUpdated >= DateTime.Now.AddDays(-14))
			{
				return part;
			}

			part = UpdatePartFromApi(number, type, part);
			return part;
		}

		/// <summary>
		/// Gets a part from the API, returns it as a Part. No relation to the DB.
		/// </summary>
		/// <param name="part">A Part entity</param>
		/// <param name="number">Part Number</param>
		/// <param name="type">Type - PART, MINIFIG, etc.</param>
		/// <returns>A Part entity</returns>
		private Part UpdatePartFromApi(string number, string type, Part part = null)
		{
			if (part == null)
			{
				part = new Part();
			}

			var response = _api.GetRequest<GetItemResponse>($"items/{type}/{number}");

			part.Number = response.data.no;
			part.Name = response.data.name;
			part.CategoryId = response.data.category_id;
			part.ImageUrl = response.data.image_url;
			part.ThumbnailUrl = response.data.thumbnail_url;
			part.Weight = response.data.weight;
			part.Description = response.data.description;
			part.LastUpdated = DateTime.Now;
			part.ItemType = response.data.type;

			return part;
		}

		/// <summary>
		/// Adds an order into the DB. If it already exists, return it.
		/// </summary>
		/// <param name="order">Order response model from API</param>
		/// <param name="orderItems">Order items response model from API</param>
		/// <returns>A BL Order entity</returns>
		public Data.Entities.BricklinkOrder AddOrder(GetOrderResponseModel order, GetOrderItemsResponseModel orderItems)
		{
			var o = _orderRepo.FindOne(x => x.OrderId == order.data.order_id.ToString()) as Data.Entities.BricklinkOrder;
			if (o != null)
			{
				return o;
			}

			var orderEntity = new Data.Entities.BricklinkOrder
			{
				TotalCount = order.data.total_count,
				UniqueCount = order.data.unique_count,
				Weight = order.data.total_weight,
				DriveThruSent = false,
				ShippingMethod = order.data.shipping?.method,
				OrderId = order.data.order_id.ToString(),
				OrderDate = order.data.date_ordered,
				BuyerName = order.data.buyer_name,
				BuyerEmail = order.data.buyer_email,
				Subtotal = decimal.Parse(order.data.cost.subtotal),
				Deductions = decimal.Parse(order.data.cost.credit) + decimal.Parse(order.data.cost.coupon),
				Shipping = decimal.Parse(order.data.cost.shipping),
				ExtraCosts = decimal.Parse(order.data.cost.salesTax) + decimal.Parse(order.data.cost.vat_amount) + 
					decimal.Parse(order.data.cost.etc1) + decimal.Parse(order.data.cost.etc2) + decimal.Parse(order.data.cost.insurance),
				GrandTotal = decimal.Parse(order.data.cost.grand_total),
				Status = order.data.status == "CANCELLED" ? OrderStatus.Cancelled : OrderStatus.Complete
			};

			var orderItemEntities = orderItems.data
				.SelectMany(x => x)
				.Select(x => new BricklinkOrderItem
			{
				Order = orderEntity,
				Name = x.item.name,
				Quantity = x.quantity,
				UnitPrice = decimal.Parse(x.unit_price_final),
				Part = GetPartModel(x.item.no, x.color_id, x.item.type, condition: x.new_or_used, updateInvDate: order.data.date_ordered).PartInventory
			}).ToList();

			orderEntity = _orderRepo.AddOrderWithItems(orderEntity, orderItemEntities);

			return _orderRepo.FindOne(x => x.Id == orderEntity.Id) as Data.Entities.BricklinkOrder;
		}

		#region api

		/// <summary>
		/// Gets pricing from the api
		/// </summary>
		/// <param name="price">Entity</param>
		/// <param name="number"></param>
		/// <param name="type"></param>
		/// <param name="colourId"></param>
		/// <param name="condition"></param>
		/// <returns></returns>
		private PartPriceInfo UpdatePartPricingFromApi(string number, string type, int colourId, string condition = "N", PartPriceInfo price = null)
		{
			if (price == null)
			{
				price = new PartPriceInfo();
			}

			var response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&country_code=UK&new_or_used={condition}&color_id={colourId}");

			var loc = "";

			// Fall back to EU
			if (response.data.avg_price == "0.0000")
			{
				response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&region=europe&new_or_used={condition}&color_id={colourId}");
				loc = "(EU)";
			}

			// Fall back to world
			if (response.data.avg_price == "0.0000")
			{
				response = _api.GetRequest<GetPriceGuideResponse>($"items/{type}/{number}/price?guide_type=sold&currency_code=GBP&vat=Y&new_or_used={condition}&color_id={colourId}");
				loc = "(World)";
			}

			if (decimal.TryParse(response.data.avg_price, out decimal tmp))
			{
				price.AveragePrice = tmp;
			}
			else
			{
				price.AveragePrice = 0;
			}
			price.AveragePriceLocation = loc;
			price.LastUpdated = DateTime.Now;

			return price;
		}

		/// <summary>
		/// Orders contain some inventory info, so update what we can from there
		/// </summary>
		/// <param name="inv">The inventory entity</param>
		/// <param name="remarks">BL Remark, becomes location</param>
		/// <param name="unit_price_final">The price this part is listed on BL</param>
		/// <param name="description">Description</param>
		/// <param name="inventory_id">Inventory id</param>
		public void UpdatePartInventoryFromOrder(PartInventory inv, string remarks, string unit_price_final, string description, int inventory_id)
		{
			if (inv.Quantity == 0)
			{
				inv.Location = "";
			}
			else
			{
				inv.Location = remarks;
			}
			
			inv.MyPrice = decimal.Parse(unit_price_final);
			inv.Description = description;

			if (inv.InventoryId == 0)
			{
				inv.InventoryId = inventory_id;
			}

			_partInventoryRepo.Update(inv);
		}

		/// <summary>
		/// Given an existing inventory entity, updates it from the api
		/// </summary>
		/// <param name="partInv">An inventory entity</param>
		/// <returns>The same entity, updated</returns>
		private PartInventory UpdateInventoryFromApi(PartInventory partInv)
		{
			return UpdateInventoryFromApi(partInv.Part.ItemType, partInv.Part.CategoryId, partInv.ColourId, partInv.Part.Number, partInv.Condition, partInv);
		}

		/// <summary>
		/// Gets an inventory item from the api, puts it into an entity
		/// </summary>
		/// <param name="type"></param>
		/// <param name="categoryId"></param>
		/// <param name="colourId"></param>
		/// <param name="number"></param>
		/// <param name="condition"></param>
		/// <param name="partInv"></param>
		/// <returns></returns>
		private PartInventory UpdateInventoryFromApi(string type, int categoryId, int colourId, string number, string condition = "N", PartInventory partInv = null)
		{
			if (partInv == null)
			{
				partInv = new PartInventory();
			}

			var response = _api.GetRequest<GetInventoriesResponseModel>($"inventories?item_type={type}&category_id={categoryId}&color_id={colourId}");

			var item = response.data.FirstOrDefault(x => x.item.no == number && x.new_or_used == condition);

			if (item == null)
			{
				partInv.InventoryId = 0;
				partInv.Quantity = 0;
				partInv.MyPrice = 0;
				partInv.ColourId = colourId;
				partInv.ColourName = Statics.Colours[colourId].Name;
				partInv.Condition = condition;
				partInv.Location = "";
				partInv.LastUpdated = DateTime.Now;
				partInv.Image = GetItemImage(type, number, colourId);

				return partInv;
			}

			partInv.InventoryId = item.inventory_id;
			partInv.Quantity = item.quantity;
			partInv.MyPrice = decimal.Parse(item.unit_price);
			partInv.ColourId = colourId;
			partInv.ColourName = Statics.Colours[colourId].Name;
			partInv.Condition = condition;
			partInv.Location = item.remarks;
			partInv.Description = item.description;
			partInv.Image = GetItemImage(type, number, colourId);

			if (string.IsNullOrEmpty(partInv.Location))
			{
				partInv.Location = item.description;
			}

			partInv.LastUpdated = DateTime.Now;

			return partInv;
		}

		#endregion
	}
}
