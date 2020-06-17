using CoolCatCollects.Data.Entities;
using CoolCatCollects.Data.Repositories;
using CoolCatCollects.Ebay.Models;
using CoolCatCollects.Ebay.Models.Responses;
using System.Linq;

namespace CoolCatCollects.Ebay
{
	public class eBayDataService
	{
		private readonly OrderRepository _orderRepo;
		private readonly BaseRepository<EbayOrderItem> _orderItemRepo;

		public eBayDataService()
		{
			_orderRepo = new OrderRepository();
			_orderItemRepo = new BaseRepository<EbayOrderItem>();
		}

		public EbayOrder AddOrder(GetOrderResponseModel obj)
		{
			var o = _orderRepo.FindOne(x => x.OrderId == obj.orderId) as EbayOrder;
			if (o != null)
			{
				return o;
			}

			var entity = new EbayOrder
			{
				LegacyOrderId = obj.legacyOrderId,
				SalesRecordReference = obj.salesRecordReference,
				OrderId = obj.orderId,
				OrderDate = obj.creationDate,
				BuyerUsername = obj.buyer.username,
				Subtotal = decimal.Parse(obj.pricingSummary.priceSubtotal.convertedFromValue),
				Shipping = decimal.Parse(obj.pricingSummary.deliveryCost.convertedFromValue),
				Deductions = decimal.Parse(obj.pricingSummary.priceDiscount?.convertedFromValue ?? "0"),
				ExtraCosts = decimal.Parse(obj.pricingSummary.adjustment?.convertedFromValue ?? "0"),
				GrandTotal = decimal.Parse(obj.pricingSummary.total.convertedFromValue),
				Status = obj.cancelStatus.cancelState != "NONE_REQUESTED" ? OrderStatus.Cancelled : obj.orderFulfillmentStatus == "NOT_STARTED" ? OrderStatus.InProgress : OrderStatus.Complete
			};

			if (obj.fulfillmentStartInstructions != null && obj.fulfillmentStartInstructions.Any() && obj.fulfillmentStartInstructions?[0].shippingStep?.shipTo != null)
			{
				entity.BuyerName = obj.fulfillmentStartInstructions?[0].shippingStep.shipTo.fullName;
				entity.BuyerEmail = obj.fulfillmentStartInstructions?[0].shippingStep.shipTo.email;
			}

			var items = obj.lineItems.Select(x => new EbayOrderItem
			{
				Order = entity,
				LineItemId = x.lineItemId,
				LegacyItemId = x.legacyItemId,
				LegacyVariationId = x.legacyVariationId ?? "0",
				SKU = x.sku,
				Image = "",
				CharacterName = "",
				Name = x.title,
				Quantity = x.quantity,
				UnitPrice = decimal.Parse(x.lineItemCost.convertedFromValue)
			}).ToList();

			entity = _orderRepo.AddOrderWithItems(entity, items);

			return _orderRepo.FindOne(x => x.Id == entity.Id) as EbayOrder;
		}

		public EbayOrderItem UpdateOrderItem(EbayOrderItem item)
		{
			return _orderItemRepo.Update(item);
		}

		public EbayOrderItem GetOrderItem(string legacyItemId, string legacyVariationId)
		{
			return _orderItemRepo.FindOne(x => x.LegacyItemId == legacyItemId && x.LegacyVariationId == legacyVariationId);
		}

		public void UpdateOrderItemsByLegacyId(string legacyItemId, string legacyVariationId, GetItemModel model)
		{
			var items = _orderItemRepo.Find(x => x.LegacyItemId == legacyItemId && x.LegacyVariationId == legacyVariationId);

			foreach(var item in items)
			{
				item.Image = model.Image;
				item.CharacterName = model.Character;

				_orderItemRepo.Update(item);
			}
		}
	}
}
