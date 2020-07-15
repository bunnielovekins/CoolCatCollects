using CoolCatCollects.Core;
using CoolCatCollects.Ebay.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoolCatCollects.Ebay.Models
{
	public class PaginationModel
	{
		public PaginationModel()
		{

		}

		public PaginationModel(int total, int page, int perPage)
		{
			Total = total;
			Page = page;
			ItemsPerPage = perPage;

			TotalPages = total / perPage + (total % perPage > 0 ? 1 : 0);
			Pages = Enumerable.Range(1, TotalPages);

			HasPrevious = page > 1;
			PreviousPage = page - 1;
			HasNext = page < TotalPages;
			NextPage = page + 1;
		}

		public int Total { get; set; }
		public int Page { get; set; }
		public int ItemsPerPage { get; set; }
		public int TotalPages { get; set; }
		public bool HasPrevious { get; set; }
		public int PreviousPage { get; set; }
		public bool HasNext { get; set; }
		public int NextPage { get; set; }
		public IEnumerable<int> Pages { get; set; }
	}

	public class EbayOrdersListModel
	{
		public EbayOrdersListModel()
		{
		}

		public EbayOrdersListModel(GetOrdersResponseModel data, int page, int perPage)
		{
			Total = data.total;
			Page = page;
			Orders = data.orders.Select(x => new EbayOrdersListItemModel(x));

			Pagination = new PaginationModel(data.total, page, perPage);
		}

		public EbayOrdersListModel(IEnumerable<EbayOrdersListItemModel> items, int page, int perPage)
		{
			Total = items.Count();
			Page = page;
			Orders = items;

			Pagination = new PaginationModel(items.Count(), page, perPage);
		}

		public IEnumerable<EbayOrdersListItemModel> Orders { get; set; }
		public int Total { get; set; }
		public int Page { get; set; }

		public PaginationModel Pagination { get; set; }
	}

	public class EbayOrdersListItemModel
	{
		public EbayOrdersListItemModel()
		{

		}

		public EbayOrdersListItemModel(GetOrderResponseModel data)
		{
			OrderId = data.orderId;
			LegacyOrderId = data.legacyOrderId;
			OrderDate = data.creationDate;
			Status = data.orderFulfillmentStatus;
			BuyerUsername = data.buyer.username;
			PriceSubtotal = data.pricingSummary.priceSubtotal.ToString();
			PriceDiscount = data.pricingSummary.priceDiscount?.ToString();
			PriceDelivery = StaticFunctions.FormatCurrencyStr(decimal.Parse(data.pricingSummary.deliveryCost.convertedFromValue) - decimal.Parse(data.pricingSummary.deliveryDiscount?.convertedFromValue ?? "0"));
			PriceTotal = data.pricingSummary.total.ToString();
			if (data.fulfillmentStartInstructions.Any() && data.fulfillmentStartInstructions[0].shippingStep != null)
			{
				BuyerName = data.fulfillmentStartInstructions[0].shippingStep.shipTo.fullName;
				ShippingMethod = GetShippingMethod(data.fulfillmentStartInstructions[0].shippingStep.shippingServiceCode);
			}
			ItemCount = data.lineItems.Sum(x => x.quantity);

			Items = data.lineItems.Select(x => new EbayOrdersListItemItemModel(x));
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

		public string OrderId { get; set; }
		public string LegacyOrderId { get; set; }
		public DateTime OrderDate { get; set; }
		public string Status { get; set; }
		public string BuyerUsername { get; set; }
		public string PriceSubtotal { get; set; }
		public string PriceDiscount { get; set; }
		public string PriceDelivery { get; set; }
		public string PriceTotal { get; set; }
		public string BuyerName { get; set; }
		public string ShippingMethod { get; set; }
		public int ItemCount { get; set; }
		public IEnumerable<EbayOrdersListItemItemModel> Items { get; set; }
	}

	public class EbayOrdersListItemItemModel
	{
		public EbayOrdersListItemItemModel()
		{

		}

		public EbayOrdersListItemItemModel(Lineitem item)
		{
			LineItemId = item.lineItemId;
			LegacyItemId = item.legacyItemId;
			LegacyVariationId = item.legacyVariationId ?? "0";
			Name = item.title;
			Cost = item.lineItemCost.ToString();
			Quantity = item.quantity;
		}

		public int OrderItemId { get; set; }
		public string LineItemId { get; set; }
		public string LegacyItemId { get; set; }
		public string LegacyVariationId { get; set; }
		public string Name { get; set; }
		public string Cost { get; set; }
		public int Quantity { get; set; }
		public string Image { get; set; }
		public string Character { get; set; }
	}
}
