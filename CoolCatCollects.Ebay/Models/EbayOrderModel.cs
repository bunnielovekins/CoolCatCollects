using CoolCatCollects.Core;
using CoolCatCollects.Ebay.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Ebay.Models
{
	public class EbayOrderModel
	{
		public EbayOrderModel()
		{

		}

		public EbayOrderModel(GetOrderResponseModel data, Data.Entities.EbayOrder orderEntity)
		{
			if (data.fulfillmentStartInstructions.Any() && data.fulfillmentStartInstructions[0].shippingStep?.shipTo?.contactAddress?.countryCode != "GB")
			{
				InitInternational(data, orderEntity);
			}
			else
			{
				Init(data, orderEntity);
			}
		}

		private void InitInternational(GetOrderResponseModel data, Data.Entities.EbayOrder orderEntity)
		{
			IsInternationalOrder = true;
			OrderNumber = data.orderId;
			OrderDate = data.creationDate.ToString("yyyy-MM-dd");
			OrderPaid = data.paymentSummary.payments[0].paymentDate.ToString("yyyy-MM-dd");
			SubTotal = StaticFunctions.FormatCurrencyStr(data.pricingSummary.priceSubtotal.convertedFromValue);
			PostagePackaging = StaticFunctions.FormatCurrencyStr(decimal.Parse(data.pricingSummary.deliveryCost.convertedFromValue) - decimal.Parse(data.pricingSummary.deliveryDiscount?.convertedFromValue ?? "0"));
			if (data.pricingSummary.adjustment != null)
			{
				Discount = data.pricingSummary.adjustment.ToString();
			}
			else
			{
				Discount = "£0.00";
			}
			Total = StaticFunctions.FormatCurrencyStr(data.pricingSummary.total.convertedFromValue);
			if (data.pricingSummary.tax != null)
			{
				Tax = StaticFunctions.FormatCurrencyStr(data.pricingSummary.tax.convertedFromValue);
			}
			if (data.fulfillmentStartInstructions.Any() && data.fulfillmentStartInstructions[0].shippingStep?.shipTo != null)
			{
				Buyer = new EbayOrderModelBuyer(data.fulfillmentStartInstructions[0].shippingStep.shipTo);
			}
			Items = data.lineItems.Select(x => new EbayOrderModelItem(x, orderEntity, true));

			HasDiscount = string.IsNullOrEmpty(Discount) || Items.Any(x => string.IsNullOrEmpty(x.Discount));
			HasVariants = Items.Any(x => !string.IsNullOrEmpty(x.LegacyVariationId));
		}

		private void Init(GetOrderResponseModel data, Data.Entities.EbayOrder orderEntity)
		{
			IsInternationalOrder = false;
			OrderNumber = data.orderId;
			OrderDate = data.creationDate.ToString("yyyy-MM-dd");
			OrderPaid = data.paymentSummary.payments[0].paymentDate.ToString("yyyy-MM-dd");
			SubTotal = StaticFunctions.FormatCurrencyStr(data.pricingSummary.priceSubtotal.convertedFromValue);
			PostagePackaging = StaticFunctions.FormatCurrencyStr(decimal.Parse(data.pricingSummary.deliveryCost.convertedFromValue) - decimal.Parse(data.pricingSummary.deliveryDiscount?.convertedFromValue ?? "0"));
			if (data.pricingSummary.priceDiscount != null)
			{
				Discount = data.pricingSummary.priceDiscount.ToString();
			}
			else
			{
				Discount = "£0.00";
			}
			Total = StaticFunctions.FormatCurrencyStr(data.pricingSummary.total.convertedFromValue);
			if (data.pricingSummary.tax != null)
			{
				Tax = StaticFunctions.FormatCurrencyStr(data.pricingSummary.tax.convertedFromValue);
			}
			if (data.fulfillmentStartInstructions.Any() && data.fulfillmentStartInstructions[0].shippingStep?.shipTo != null)
			{
				Buyer = new EbayOrderModelBuyer(data.fulfillmentStartInstructions[0].shippingStep.shipTo);
			}
			Items = data.lineItems.Select(x => new EbayOrderModelItem(x, orderEntity, false));

			HasDiscount = string.IsNullOrEmpty(Discount) || Items.Any(x => string.IsNullOrEmpty(x.Discount));
			HasVariants = Items.Any(x => !string.IsNullOrEmpty(x.LegacyVariationId));
		}

		public string OrderNumber { get; set; }
		public string OrderDate { get; set; }
		public string OrderPaid { get; set; }
		public string SubTotal { get; set; }
		public string PostagePackaging { get; set; }
		public string Discount { get; set; }
		public string Tax { get; set; }
		public string Total { get; set; }
		public EbayOrderModelBuyer Buyer { get; set; }
		public IEnumerable<EbayOrderModelItem> Items { get; set; }
		public bool HasDiscount { get; set; }
		public bool HasVariants { get; set; }
		public bool IsInternationalOrder { get; set; }

		public class EbayOrderModelBuyer
		{
			public EbayOrderModelBuyer()
			{

			}

			public EbayOrderModelBuyer(Shipto shipto)
			{
				Name = shipto.fullName;
				Address1 = shipto.contactAddress.addressLine1;
				Address2 = shipto.contactAddress.city;
				PostCode = shipto.contactAddress.postalCode;
				Country = shipto.contactAddress.countryCode;
			}

			public string Name { get; set; }
			public string Address1 { get; set; }
			public string Address2 { get; set; }
			public string PostCode { get; set; }
			public string Country { get; set; }
		}

		public class EbayOrderModelItem
		{
			public EbayOrderModelItem()
			{

			}

			public EbayOrderModelItem(Lineitem item, Data.Entities.EbayOrder orderEntity, bool international)
			{
				var entity = orderEntity.OrderItems.Cast<Data.Entities.EbayOrderItem>().FirstOrDefault(x => 
					x.LegacyItemId == item.legacyItemId && 
					x.LegacyVariationId == (item.legacyVariationId ?? "0") &&
					x.LineItemId == item.lineItemId);

				if (international)
				{
					InitInternational(item, entity);
				}
				else
				{
					Init(item, entity);
				}
			}

			private void InitInternational(Lineitem item, Data.Entities.EbayOrderItem entity)
			{
				Id = item.lineItemId;
				Name = item.title;
				UnitPrice = GetUnitCost(item.lineItemCost.convertedFromValue, item.quantity);
				Discount = "£0.00";
				Quantity = item.quantity;
				SubTotal = item.lineItemCost.ToString();
				LegacyVariationId = item.legacyVariationId ?? "0";
				LegacyItemId = item.legacyItemId;
				Variant = entity.CharacterName;
				Image = entity.Image;
			}

			private void Init(Lineitem item, Data.Entities.EbayOrderItem entity)
			{
				Id = item.lineItemId;
				Name = item.title;
				UnitPrice = item.lineItemCost.ToString();
				if (item.appliedPromotions.Any() && item.appliedPromotions[0].discountAmount != null)
				{
					Discount = StaticFunctions.FormatCurrencyStr(item.appliedPromotions[0].discountAmount.convertedFromValue);
				}
				else
				{
					Discount = "£0.00";
				}
				Quantity = item.quantity;
				SubTotal = StaticFunctions.FormatCurrencyStr(item.total.convertedFromValue);
				LegacyVariationId = item.legacyVariationId ?? "0";
				LegacyItemId = item.legacyItemId;
				Variant = entity.CharacterName;
				Image = entity.Image;
			}

			private string GetUnitCost(string lineItemCost, int quantity)
			{
				var d = StaticFunctions.FormatCurrency(lineItemCost);
				return StaticFunctions.FormatCurrencyStr(d / quantity);
			}

			public string Id { get; set; }
			public string Name { get; set; }
			public string UnitPrice { get; set; }
			public string Discount { get; set; }
			public int Quantity { get; set; }
			public string SubTotal { get; set; }
			public string LegacyItemId { get; set; }
			public string LegacyVariationId { get; set; }
			public string Variant { get; set; }
			public string Image { get; set; }
		}
	}
}
