using CoolCatCollects.Core;
using System;
using System.Collections.Generic;

namespace CoolCatCollects.Ebay.Models.Responses
{
	public class GetOrderResponseModel
	{
		public string orderId { get; set; }
		public string legacyOrderId { get; set; }
		public DateTime creationDate { get; set; }
		public DateTime lastModifiedDate { get; set; }
		public string orderFulfillmentStatus { get; set; }
		public string orderPaymentStatus { get; set; }
		public string sellerId { get; set; }
		public Buyer buyer { get; set; }
		public Pricingsummary pricingSummary { get; set; }
		public Cancelstatus cancelStatus { get; set; }
		public Paymentsummary paymentSummary { get; set; }
		public Fulfillmentstartinstruction[] fulfillmentStartInstructions { get; set; }
		public string[] fulfillmentHrefs { get; set; }
		public IEnumerable<Lineitem> lineItems { get; set; }
		public string salesRecordReference { get; set; }
	}

	public class Buyer
	{
		public string username { get; set; }
	}

	public class Pricingsummary
	{
		public PriceValue priceSubtotal { get; set; }
		public PriceValue priceDiscount { get; set; }
		public PriceValue deliveryCost { get; set; }
		public PriceValue adjustment { get; set; }
		public PriceValue total { get; set; }
	}

	public class PriceValue
	{
		public string value { get; set; }
		public string currency { get; set; }
		public string convertedFromValue { get; set; }
		public string convertedFromCurrency { get; set; }

		public override string ToString()
		{
			return StaticFunctions.FormatCurrencyStr(convertedFromValue);
		}
	}

	public class Cancelstatus
	{
		public string cancelState { get; set; }
		public object[] cancelRequests { get; set; }
	}

	public class Paymentsummary
	{
		public PriceValue totalDueSeller { get; set; }
		public object[] refunds { get; set; }
		public Payment[] payments { get; set; }
	}

	public class Payment
	{
		public string paymentMethod { get; set; }
		public string paymentReferenceId { get; set; }
		public DateTime paymentDate { get; set; }
		public PriceValue amount { get; set; }
		public string paymentStatus { get; set; }
		public Paymenthold[] paymentHolds { get; set; }
	}

	public class Paymenthold
	{
		public PriceValue holdAmount { get; set; }
		public string holdState { get; set; }
		public DateTime releaseDate { get; set; }
	}

	public class Fulfillmentstartinstruction
	{
		public string fulfillmentInstructionsType { get; set; }
		public DateTime minEstimatedDeliveryDate { get; set; }
		public DateTime maxEstimatedDeliveryDate { get; set; }
		public bool ebaySupportedFulfillment { get; set; }
		public Shippingstep shippingStep { get; set; }
	}

	public class Shippingstep
	{
		public Shipto shipTo { get; set; }
		public string shippingCarrierCode { get; set; }
		public string shippingServiceCode { get; set; }
	}

	public class Shipto
	{
		public string fullName { get; set; }
		public Contactaddress contactAddress { get; set; }
		public Primaryphone primaryPhone { get; set; }
		public string email { get; set; }
	}

	public class Contactaddress
	{
		public string addressLine1 { get; set; }
		public string city { get; set; }
		public string stateOrProvince { get; set; }
		public string postalCode { get; set; }
		public string countryCode { get; set; }
	}

	public class Primaryphone
	{
		public string phoneNumber { get; set; }
	}

	public class Lineitem
	{
		public string lineItemId { get; set; }
		public string legacyItemId { get; set; }
		public string legacyVariationId { get; set; }
		public string sku { get; set; }
		public string title { get; set; }
		public PriceValue lineItemCost { get; set; }
		public PriceValue discountedLineItemCost { get; set; }
		public int quantity { get; set; }
		public string soldFormat { get; set; }
		public string listingMarketplaceId { get; set; }
		public string purchaseMarketplaceId { get; set; }
		public string lineItemFulfillmentStatus { get; set; }
		public PriceValue total { get; set; }
		public Deliverycost1 deliveryCost { get; set; }
		public Appliedpromotion[] appliedPromotions { get; set; }
		public object[] taxes { get; set; }
		public Properties properties { get; set; }
		public Lineitemfulfillmentinstructions lineItemFulfillmentInstructions { get; set; }
	}

	public class Deliverycost1
	{
		public PriceValue shippingCost { get; set; }
	}

	public class Properties
	{
		public bool buyerProtection { get; set; }
	}

	public class Lineitemfulfillmentinstructions
	{
		public DateTime minEstimatedDeliveryDate { get; set; }
		public DateTime maxEstimatedDeliveryDate { get; set; }
		public DateTime shipByDate { get; set; }
		public bool guaranteedDelivery { get; set; }
	}

	public class Appliedpromotion
	{
		public PriceValue discountAmount { get; set; }
		public string promotionId { get; set; }
		public string description { get; set; }
	}
}
