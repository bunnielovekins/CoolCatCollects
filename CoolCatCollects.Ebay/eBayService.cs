using CoolCatCollects.Data.Entities;
using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace CoolCatCollects.Ebay
{
	public class eBayService
	{
		private readonly eBayApiService _service;
		private readonly eBayDataService _dataService;

		public eBayService()
		{
			_service = new eBayApiService();
			_dataService = new eBayDataService();
		}

		public EbayOrderModel GetOrder(string orderNumber)
		{
			var response = _service.GetRequest($"sell/fulfillment/v1/order/{orderNumber}");

			var obj = JsonConvert.DeserializeObject<GetOrderResponseModel>(response);

			var orderEntity = _dataService.AddOrder(obj);

			bool updated = false;
			foreach (var item in orderEntity.OrderItems.Cast<EbayOrderItem>())
			{
				if (!string.IsNullOrEmpty(item.Image))
				{
					continue;
				}

				updated = true;

				var itemFromApi = GetItem(item.LegacyItemId, item.LegacyVariationId);

				item.Image = itemFromApi.Image;
				item.CharacterName = itemFromApi.Character;
			}

			if (updated)
			{
				orderEntity = _dataService.AddOrder(obj);
			}

			return new EbayOrderModel(obj, orderEntity);
		}

		public EbayOrdersListModel GetOrders(int limit, int page)
		{
			int offset = (page - 1) * 50;
			var response = _service.GetRequest("/sell/fulfillment/v1/order" +
				//$"?filter=orderfulfillmentstatus:{{{status}}}" + {FULFILLED|IN_PROGRESS} {NOT_STARTED|IN_PROGRESS}
				$"?limit={limit}" +
				$"&offset={offset}");

			var obj = JsonConvert.DeserializeObject<GetOrdersResponseModel>(response);

			var items = obj.orders.Select(data =>
			{
				var mod = new EbayOrdersListItemModel
				{
					OrderId = data.orderId,
					LegacyOrderId = data.legacyOrderId,
					OrderDate = data.creationDate,
					Status = data.orderFulfillmentStatus,
					BuyerUsername = data.buyer.username,
					PriceSubtotal = data.pricingSummary.priceSubtotal.ToString(),
					PriceDiscount = data.pricingSummary.priceDiscount?.ToString(),
					PriceDelivery = data.pricingSummary.deliveryCost.ToString(),
					PriceTotal = data.pricingSummary.total.ToString(),
					ItemCount = data.lineItems.Sum(x => x.quantity),
					Items = data.lineItems.Select(x => new EbayOrdersListItemItemModel(x))
				};

				if (data.fulfillmentStartInstructions.Any() && data.fulfillmentStartInstructions[0].shippingStep != null)
				{
					mod.BuyerName = data.fulfillmentStartInstructions[0].shippingStep.shipTo.fullName;
					mod.ShippingMethod = GetShippingMethod(data.fulfillmentStartInstructions[0].shippingStep.shippingServiceCode);
				}

				var entity = _dataService.AddOrder(data);

				mod.Items = mod.Items.Select(x =>
				{
					var ent = entity.OrderItems.Cast<EbayOrderItem>().FirstOrDefault(y => y.LegacyItemId == x.LegacyItemId && y.LineItemId == x.LineItemId && y.LegacyVariationId == x.LegacyVariationId);

					if (ent == null)
					{
						return x;
					}

					x.OrderItemId = ent.Id;
					if (!string.IsNullOrEmpty(ent.Image))
					{
						x.Image = ent.Image;
						x.Character = ent.CharacterName;
					}

					return x;
				});

				return mod;
			});

			var model = new EbayOrdersListModel(items, page, limit);

			return model;
		}

		public string GetShippingMethod(string method)
		{
			switch (method)
			{
				case "UK_RoyalMailSecondClassStandard":
					return "RM Second Class";
				case "UK_RoyalMailFirstClassStandard":
					return "RM First Class";
				case "UK_RoyalMailAirmailInternational":
					return "RM International Standard";
				case "UK_eBayDeliveryPacklinkIntl":
					return "eBay Packlink International";
				case "UK_RoyalMailSecondClassRecorded":
					return "RM Second Class Recorded";
				case "UK_RoyalMailFirstClassRecorded":
					return "RM First Class Recorded";
			}

			return method;
		}

		public GetItemModel GetItem(string legacyItemId, string legacyVariationId)
		{
			if (string.IsNullOrEmpty(legacyVariationId))
			{
				legacyVariationId = "0";
			}

			var response = _service.GetRequest($"https://api.ebay.com/buy/browse/v1/item/v1|{legacyItemId}|{legacyVariationId}");

			var obj = JsonConvert.DeserializeObject<GetItemResponse>(response);

			var character = "";
			var sku = "";
			foreach (var aspect in obj.localizedAspects)
			{
				if (aspect.name == "Character")
				{
					character = aspect.value;
				}
				else if (aspect.name == "SKU")
				{
					sku = aspect.value;
				}
			}

			var model = new GetItemModel
			{
				Character = character,
				Image = obj.image.imageUrl,
				SKU = sku
			};

			_dataService.UpdateOrderItemsByLegacyId(legacyItemId, legacyVariationId, model);

			return model;
		}
	}
}
